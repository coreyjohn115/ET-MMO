"""
公式
"""

import common.base as base


class __formula:
    def sum(*args):
        """
        累加
        """
        s = 0
        for k, v in enumerate(args[0]):
            s = s + base.to_int(v)
        return s

    def plus(*args):
        """
        累乘
        """
        s = 1
        for k, v in enumerate(args[0]):
            s = s * base.to_int(v)
        return s

def get_formula_value(t, row):
    if t["start_col"] >= t["end_col"]:
        return row[t["start_col"]-1] or 0
    f_name = row[t["start_col"]-1]
    if f_name == "":
        return 0
    args = []
    arg_k = t["start_col"]
    while(arg_k < t["end_col"]):
        args.append(row[arg_k])
        arg_k = arg_k + 1
    if f_name in vars(__formula):
        return eval("__formula."+f_name+"(args)")
    base.error("找不到公式", f_name)
    return 0