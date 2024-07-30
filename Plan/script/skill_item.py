"""
技能道具模块
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

def build_skill_item(file_name, sheet_name, dst_file_name, dst_sheet_name):
    """ 对当前表生成结果 """
    res_data_list = base.get_build_data(file_name, sheet_name)
    # skill_item
    item_list = []
    level_list = []
    star_list = []
    for k, v in enumerate(res_data_list):
        master_item = {}
        master_item['id'] = base.to_int(v['id'])
        name_language_id = base.add_language(v['name'])
        master_item['name'] = name_language_id
        desc_language_id = base.add_language(v['desc'])
        master_item['desc'] = desc_language_id
        master_item['icon'] = v['icon']
        master_item['quality'] = v['quality']
        master_item['weapon_mask'] = v['weapon_mask']
        master_item['classify'] = v['classify']
        weapon_desc_language_id = base.add_language(v['weapon_desc'])
        master_item['weapon_desc'] = weapon_desc_language_id
        master_item['drop_object_id'] = v['drop_object_id']
        master_item['type'] = v['type']
        master_item['decompose_id'] = v['decompose_id']
        master_item['slot'] = v['slot']
        master_item['auto_equip'] = v['auto_equip']
        item_list.append(master_item)
        for level in range(1, 4):
            level_skill = {}
            level_skill['id'] = master_item['id'] * 1000 + level
            level_skill['itemid'] = master_item['id']
            level_skill['level'] = level
            str1 = str(base.to_int(v['attr_list']))
            str2 = str(base.to_int(v['attr_count']))
            level_skill['attr_list'] = "{[" + str1 + "]=" + str2 + "}"
            level_skill['consume_list'] = "{{['itemid']=" + v['_ss_id'] + ",['count']=" + v['_ss_count'] + "}}"
            level_skill['consume_id'] = v['consume_id']
            level_skill['consume_count'] = base.to_int(v['consume_count']) + level - 1
            level_list.append(level_skill)
            star_skill = {}
            star_skill['id'] = master_item['id'] * 1000 + level
            star_skill['itemid'] = master_item['id']
            star_skill['star'] = level
            star_skill['skill_id'] = v['skill_id']
            star_skill['consume_list'] = "{{['itemid']=" + v['_star_id'] + ",['count']=" + v['_star_count'] + "}}"
            star_skill['consume_id'] = v['_star_consume']
            star_skill['consume_count'] = base.to_int(v['_star_count']) + level - 1
            star_skill['name'] = master_item['name']
            star_skill['desc'] = master_item['desc']
            str3 = str(base.to_int(v['atlas_attr_id']))
            str4 = str(base.to_int(v['atlas_attr_count']))
            star_skill['attr_list'] = "{[" + str3 + "]=" + str4 + "}"
            draw_desc_language_id = base.add_language(v['atlas_attr_desc'])
            star_skill['draw_desc'] = draw_desc_language_id
            star_list.append(star_skill)
    base.fill_to_excel(item_list, "skill.xlsx", "skill")
    base.fill_to_excel(level_list, "skill.xlsx", "skill_level")
    base.fill_to_excel(star_list, "skill.xlsx", "skill_star")
    return 0

base.register_build("build_skill_item", build_skill_item)