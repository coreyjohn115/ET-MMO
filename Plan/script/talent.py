"""
天赋模块
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

def get_talent_eff(eff):
    """天赋效果"""
    obj = {}
    a = 9
    obj['Cmd'] = eff['effect_name']
    obj['OftList'] = []
    sub_obj = get_talent_oft_eff(eff)
    obj['OftList'].append(sub_obj)
    obj['Args'] = []
    for k in range(1, a):
        if (eff['eff_arg' + str(k)]) == "":
            a = a - 1
    for k in range(1, a):
        if eff['eff_arg' + str(k)] == "`0":
            eff['eff_arg' + str(k)] = "0"
        obj['Args'].append(eff['eff_arg' + str(k)] or "0")
    return obj

def get_talent_oft_eff(eff):
    """天赋修改效果"""
    obj = {}
    obj['Idx'] = base.to_int(eff['idx'] or 0)
    obj['DstList'] = []
    for k in range(1, 4):
        obj['DstList'].append(base.to_int(eff['eff_dst_list' + str(k)] or "0"))
    return obj

def build_talent(file_name, sheet_name, dst_file_name, dst_sheet_name):
    """ 对当前表生成结果 """
    res_data_list = base.get_build_data(file_name, sheet_name)
    eff_data_list = base.get_build_data(file_name, "effect")
    eff_data_map = base.list_to_map(eff_data_list, "id")
    # skill_talent
    talent_list = []
    for k, v in enumerate(res_data_list):
        is_build = base.to_int(v['_jss'])
        if is_build != 1:
            continue
        skill_talent = {}
        skill_talent['Id'] = base.to_int(v['id']) * 1000 + 1
        name_language_id = base.add_language(v['name'])
        skill_talent['Name'] = name_language_id
        desc_language_id = base.add_language(v['desc'])
        skill_talent['Desc'] = desc_language_id
        skill_talent['Icon'] = v['icon']
        skill_talent['Level'] = v['level']
        skill_talent['MasterId'] = v['master_id']
        eff_id_list = []
        if (v['_effect_list'] or "") == "":
            for kk, sub in enumerate(eff_data_list):
                if sub['_talent_name'] == v['name']:
                    eff_id_list.append(sub['id'])
        else:
            eff_id_list = v['_effect_list'].split(';')
        eff_list = []
        for _, eff_id in enumerate(eff_id_list):
            _id = str(eff_id)
            if _id not in eff_data_map:
                base.error("没有找到技能效果技能ID=%s 效果ID=%s" % (skill_talent['id'], _id))
            eff_list.append(get_talent_eff(eff_data_map[_id]))
        skill_talent['EffectList'] = base.to_json(eff_list)
        talent_list.append(skill_talent)
    base.fill_to_excel(talent_list, "TalentConfig.xlsx", "Talent")
    return 0

base.register_build("build_talent", build_talent)
