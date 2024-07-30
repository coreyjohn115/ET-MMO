"""
道具模块
"""
from pyxll import xl_func, xl_macro, xl_app, xlcAlert
import common.base as base
from pyxll import xl_func, xl_macro

def build_item(file_name, sheet_name, dst_file_name, language_name):
    """ 对当前表生成结果 """
    res_data_list = base.get_build_data(file_name, sheet_name)
    master_list = []
    for k, v in enumerate(res_data_list):
        master_item = {}
        master_item['Id'] = base.to_int(v['id'])
        name_language_id = base.add_language(v['name'])
        master_item['Name'] = name_language_id
        desc_language_id = base.add_language(v['desc'])
        master_item['Desc'] = desc_language_id
        master_item['__'] = v['name']
        master_item['Icon'] = v['icon']
        master_item['Quality'] = v['quality']
        master_item['Type'] = v['type']
        master_item['SubType'] = v['sub_type']
        master_item['AutoUse'] = v['is_auto_use']
        master_item['ItemMode'] = v['item_mode']
        master_item['ModeArgs'] = v['mode_arg']
        master_item['DropPrefab'] = v['drop_prefab']
        master_item['Stack'] = v['stack']
        master_item['LackTip'] = v['lack_language_id']
        master_list.append(master_item)
    base.fill_to_excel(master_list, "ItemConfig.xlsx", "Item")
    base.language_to_file(language_name, "Item")
    return 0

base.register_build("build_item", build_item)



