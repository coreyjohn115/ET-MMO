"""
怪物模块
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

def build_monster(file_name, sheet_name, dst_file_name, dst_sheet_name):
    """ 对当前表生成结果 """
    res_data_list = base.get_build_data(file_name, sheet_name)
    # monster_level
    level_list = []
    for k, v in enumerate(res_data_list):
        monster_attr = {}
        monster_attr['level'] = base.to_int(v['level'])
        for level in range(1, 11):
            monster_attr['id'] = base.to_int(v['id'])
            monster_attr['monster_id'] = base.to_int(v['monster_id'])
            monster_attr['level'] = level
            monster_attr['attr_list'] = v['_ss1'] + v['_ss2'] + "60:999;61:999;62:999;63:999;"
            monster_attr['drop_list'] = v['drop']
            monster_attr['special_item_list'] = v['special_item_list']
            monster_attr['first_drop_list'] = v['first_drop_list']
            monster_attr['potion_drop_list'] = v['potion_drop_list']
        level_list.append(monster_attr)
    base.fill_to_excel(level_list, "monster_level.xlsx", "level")
    return 0

base.register_build("build_monster", build_monster)