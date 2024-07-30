"""
BUFF模块
"""
from pyxll import xl_func, xl_macro, xl_app, xlcAlert
import common.base as base
from pyxll import xl_func, xl_macro

def parse_to_list(s):
    use_p_list = []
    add_p_list = []
    str_list = s.split(";")
    for _, v in enumerate(str_list):
        if v != "":
            s_list2 = v.split(":")
            if len(s_list2)>2:
                if int(s_list2[2]) > 0:
                    add_p_list.append(v)
                else:
                    s_list2[2] = str(-int(s_list2[2]))
                    use_p_list.append(":".join(str(i) for i in s_list2))
    use_s = ""
    add_s = ""
    if len(use_p_list)> 0:
        use_s = ";".join(str(i) for i in use_p_list) + ";"
    if len(add_p_list) > 0:
        add_s = ";".join(str(i) for i in add_p_list) + ";"
    return use_s, add_s

def get_buff_sub_eff(eff, idx):
    """buff子效果"""
    obj = {}
    a = 8
    idx = str(idx)
    obj['Cmd'] = eff["sub_eff" + idx] or ""
    obj['Args'] = []
    for k in range(1, a):
        if eff['sub_eff' + idx + '_arg' + str(k)] == "":
            a = a - 1
    for k in range(1, a):
        if eff['sub_eff' + idx + '_arg' + str(k)] == "`0":
            eff['sub_eff' + idx + '_arg' + str(k)] = 0
        obj['Args'].append(base.to_int(eff['sub_eff' + idx + '_arg' + str(k)] or "0"))
    return obj

def get_buff_eff(eff):
    """buff效果"""
    obj = {}
    a = 9
    obj['Cmd'] = eff['effect_name']
    obj['ViewCmd'] = eff['view_id'] or ""
    obj['Args'] = []
    for k in range(1, a):
        if eff['eff_arg' + str(k)] == "":
            a = a - 1
    for k in range(1, a):
        if eff['eff_arg' + str(k)] == "`0":
            eff['eff_arg' + str(k)] = 0
        obj['Args'].append(base.to_int(eff['eff_arg' + str(k)] or "0"))
    obj['SubList'] = []
    for k in range(1, 2):
        sub_obj = get_buff_sub_eff(eff, k)
        if sub_obj['Cmd'] != "":
            obj['SubList'].append(sub_obj)
    return obj

def build_buff(file_name, sheet_name, dst_file_name, dst_sheet_name):
    """ 对当前表生成结果 """
    res_data_list = base.get_build_data(file_name, sheet_name)
    eff_data_list = base.get_build_data(file_name, "effect")
    eff_data_map = base.list_to_map(eff_data_list, "id")
    buff_list = []
    for k, v in enumerate(res_data_list):
        is_build = base.to_int(v['_jss'])
        if is_build != 1:
            continue
        skill_buff = {}
        skill_buff['Id'] = base.to_int(v['id']) * 1000 + 1
        name_language_id = base.add_language(v['name'])
        skill_buff['Name'] = name_language_id
        desc_language_id = base.add_language(v['desc'])
        skill_buff['Desc'] = desc_language_id
        skill_buff['Icon'] = v['icon']
        skill_buff['Ms'] = v['ms']
        skill_buff['ViewCmd'] = v['view_cmd']
        skill_buff['Interval'] = v['interval']
        skill_buff['AddType'] = v['add_type']
        skill_buff['MaxLayer'] = v['max_layer']
        skill_buff['Classify'] = v['classify_map']
        skill_buff['Mutex'] = v['mutex_map']
        skill_buff['MutexLevel'] = v['mutex_lv']
        skill_buff['EffectType'] = v['effect_type']
        skill_buff['MasterId'] = v['id']
        skill_buff['Quality'] = v['quality']
        skill_buff['Element'] = v['element']
        skill_buff['AttackType'] = v['attack_type']
        eff_id_list = []
        if (v["effect_list"] or "") != "":
            eff_id_list = v["effect_list"].split(';')
        eff_list = []
        for _, eff_id in enumerate(eff_id_list):
            _id = str(eff_id)
            if _id not in eff_data_map:
                base.error("没有找到BUFF效果BUFF_ID=%s 效果ID=%s" % (skill_buff['id'], _id))
            eff_list.append(get_buff_eff(eff_data_map[_id]))
        skill_buff["EffectList"] = base.to_json(eff_list)
        buff_list.append(skill_buff)
    base.fill_to_excel(buff_list, "BuffConfig.xlsx", "Buff")
    return 0

base.register_build("build_buff", build_buff)
