"""
技能模块
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


def get_skill_sub_eff(eff, idx):
    """技能子效果"""
    obj = {}
    a = 5
    idx = str(idx)
    obj['Cmd'] = eff["sub_eff" + idx] or ""
    obj['Args'] = []
    for k in range(1, a):
        if eff['sub_eff' + idx + '_arg' + str(k)] == "":
            a = a - 1
    for k in range(1, a):
        if eff['sub_eff' + idx + '_arg' + str(k)] =="`0":
            eff['sub_eff' + idx + '_arg' + str(k)] = 0
        obj['Args'].append(base.to_int(eff['sub_eff' + idx + '_arg' + str(k)]))
    return obj

def get_skill_eff(eff):
    """技能效果"""
    obj = {}
    a = 9
    obj['Cmd'] = eff['effect_name']
    obj['Ms'] = base.to_int(eff['delay'] or 0)
    obj['Dst'] = base.to_int(eff['dst_type'])
    obj['Rate'] = eff['rate']
    obj['RangeType'] = base.to_int(eff['range_type'])
    obj['RangeArgs'] = []
    for k in range(1, 5):
        v = eff['r_args' + str(k)]
        obj['RangeArgs'].append(base.to_int(v))
    obj['ViewCmd'] = eff['view_id']
    obj['Args'] = []
    for k in range(1, a):
        if eff['eff_arg' + str(k)] == "":
            a = a - 1
    for k in range(1, a):
        if eff['eff_arg' + str(k)] == "`0":
            eff['eff_arg' + str(k)] = 0
        obj['Args'].append(base.to_int(eff['eff_arg' + str(k)] or "0"))
    obj['SubList'] = []
    for k in range(1, 4):
        sub_obj = get_skill_sub_eff(eff, k)
        if sub_obj['Cmd'] != "":
            obj['SubList'].append(sub_obj)
    return obj

def build_skill(file_name, sheet_name, dst_file_name, dst_sheet_name):
    """ 对当前表生成结果 """
    res_data_list = base.get_build_data(file_name, sheet_name)
    eff_data_list = base.get_build_data(file_name, "effect")
    eff_data_map = base.list_to_map(eff_data_list, "id")
    # master
    master_list = []
    sub_list = []
    for k, v in enumerate(res_data_list):
        is_build = base.to_int(v['_jss'])
        if is_build != 1:
            continue
        master_skill = {}
        master_skill['Id'] = base.to_int(v['id'])
        name_language_id = base.add_language(v['name'])
        master_skill['Name'] = name_language_id
        master_skill['Desc'] = name_language_id
        master_skill['Icon'] = v['icon']
        master_skill['MaxLevel'] = base.to_int(v['max_level'])
        master_skill['MaxLayer'] = v['max_layer']
        master_skill['Sort'] = v['sort']
        master_skill['HateRate'] = v['hate_rate']
        master_skill['HateBase'] = v['hate_base']
        master_skill['Classify'] = v['classify']
        master_skill['DstType'] = v['dst_type']
        master_skill['RangeType'] = v['range_type']
        master_skill['RangeArgs1'] = v['range_args1']
        master_skill['RangeArgs2'] = v['range_args2']
        master_skill['NextId'] = v['next_id']
        master_skill['MaxDistance'] = v['max_distance']
        master_skill['BestDistPercent'] = base.to_int(v['best_dist_percent']) * 100
        master_skill['Interrupt'] = v['interrupt_classify']
        master_skill['ActionName'] = v['act_name']
        master_skill['Element'] = v['element']
        master_skill['AttackType'] = v['attack_type']
        master_skill['SkillType'] = v['skill_type']
        if (v['talent_list'] or "") == "":
            master_skill['TalentList'] = ""
        else:
            master_skill['TalentList'] = v['talent_list']
        master_list.append(master_skill)

        eff_id_list = []
        if (v['_effect_list'] or "") == "":
            for kk, sub in enumerate(eff_data_list):
                if sub['_skill_name'] == v['name']:
                    eff_id_list.append(sub['id'])
        else:
            eff_id_list = v['_effect_list'].split(';')
        for level in range(1, 2):
            sub_skill = {}
            sub_skill['Id'] = master_skill['Id'] * 1000 + level
            sub_skill['MasterId'] = master_skill['Id']
            sub_skill['Level'] = level
            sub_skill['ColdTime'] = v['cd_time']
            sub_skill['SingTime'] = v['sing_time']
            sub_skill['UsePList'] = v['use_p_list']
            sub_skill['AddPList'] = v['add_p_list'] 
            eff_list = []
            for _, eff_id in enumerate(eff_id_list):
                _id = str(eff_id)
                if _id not in eff_data_map:
                    base.error("没有找到技能效果技能ID=%s 效果ID=%s" % (master_skill['id'], _id))
                eff_list.append(get_skill_eff(eff_data_map[_id]))
            skill_eff = {}
            sub_skill['EffectList'] = base.to_json(eff_list)
            sub_list.append(sub_skill)
    base.fill_to_excel(master_list, "SkillMasterConfig.xlsx", "Master")
    base.fill_to_excel(sub_list, "SkillConfig.xlsx", "Sub")
    return 0

base.register_build("build_skill", build_skill)
