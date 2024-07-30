"""
装备模块
"""
from pyxll import xl_func, xl_macro, xl_app, xlcAlert
import common.base as base
from pyxll import xl_func, xl_macro


def build_Equip(file_name, sheet_name, dst_file_name, language_name):
    """ 对当前表生成结果 """
    res_data_list = base.get_build_data(file_name, sheet_name)
    # eff_data_list = base.get_build_data(file_name, "effect")
    equip_list = []
    for k, v in enumerate(res_data_list):
        is_build = base.to_int(v['_jss'])
        if is_build != 1:
            continue
        equip = {}
        equip['Id'] = base.to_int(v['id'])
        name_language_id = base.add_language(v['name'])
        equip['Name'] = name_language_id
        desc_language_id = base.add_language(v['desc'])
        equip['Desc'] = desc_language_id
        equip['Icon'] = v['icon']
        equip['Quality'] = v['quality']
        equip['Type'] = v['type']
        equip['EquipPos'] = v['pos']
        equip['DressLevel'] = v['dress_level']
        equip['AttrList'] = ""
        equip['AttrList'] = v['base_attr_list']
        equip['RandomList'] = v['can_random_entry_list']
        equip['Skill'] = v['skill_id']
        equip['SuitId'] = v['suit_id']
        equip['WeaponType'] = v['weapon_type']
        equip['Exp'] = v['exp']
        equip_list.append(equip)
    base.fill_to_excel(equip_list, "Equip/EquipConfig.xlsx", "Equip")
    base.language_to_file(language_name, "Equip")
    return 0

base.register_build("build_equip", build_Equip)
