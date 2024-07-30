"""
卡牌模块
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

def build_card(file_name, sheet_name, dst_file_name, dst_sheet_name):
    """ 对当前表生成结果 """
    res_data_list = base.get_build_data(file_name, sheet_name)
    # card
    card_list = []
    for k, v in enumerate(res_data_list):
        card_skill = {}
        card_skill['id'] = base.to_int(v['id'])
        name_language_id = base.add_language(v['name'])
        card_skill['name'] = name_language_id
        desc_language_id = base.add_language(v['desc'])
        card_skill['desc'] = desc_language_id
        card_skill['icon'] = v['icon']
        card_skill['initial'] = v['initial']
        card_skill['level_effect'] = v['level_effect']
        card_skill['quality'] = v['quality']
        str1 = v['fetter1']
        str2 = v['fetter2']
        str3 = v['fetter3']
        card_skill['fetter'] = str1 + '3;' + str2 + '4;' + str3 + '5;'
        card_skill['rate'] = v['rate']
        card_list.append(card_skill)

    base.fill_to_excel(card_list, "abyss.xlsx", "ability")
    return 0

base.register_build("build_card", build_card)