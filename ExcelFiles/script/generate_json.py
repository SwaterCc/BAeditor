import pandas as pd
import json
import os

def generate_json(directory_path, output_file="../../Assets/BattleData/Json/BattleConfigs.json"):
    """
    读取目录下的所有 Excel 文件，并生成 JSON 文件。
    :param directory_path: 包含 Excel 文件的目录路径
    :param output_file: 输出的 JSON 文件名
    """
    # 获取目录下所有的 Excel 文件
    excel_files = [
        f for f in os.listdir(directory_path)
        if f.endswith(".xlsx") and not f.startswith("~$")
    ]

    result = {}  # 最终的 JSON 数据结构

    for file_name in excel_files:
        file_path = os.path.join(directory_path, file_name)
        print(f"Processing file: {file_path}")

        # 去掉文件后缀名作为 JSON 的键
        base_file_name = os.path.splitext(file_name)[0]

        # 初始化当前文件的数据结构
        result[base_file_name] = {}

        # 读取 Excel 文件，不自动设置列名
        excel_data = pd.ExcelFile(file_path)

        # 遍历每个工作表
        for sheet_name in excel_data.sheet_names:
            df = excel_data.parse(sheet_name, header=None)  # 不将第一行当作列名

            # 提取前三行作为表头信息
            field_descriptions = df.iloc[0].tolist()  # 第一行：描述
            field_names = df.iloc[1].tolist()         # 第二行：字段名
            field_types = df.iloc[2].tolist()         # 第三行：字段类型

            # 检查第一行是否有空数据（描述）
            for col_idx, desc in enumerate(field_descriptions):
                if pd.isnull(desc) or str(desc).strip() == "":
                    print(f"[Warning] File: {file_name}, Sheet: {sheet_name}, Column {col_idx + 1} is missing a description.")

            # 检查第二行和第三行是否有空数据（字段名和字段类型）
            for row_idx, row in enumerate([field_names, field_types]):
                for col_idx, value in enumerate(row):
                    if pd.isnull(value) or str(value).strip() == "":
                        raise ValueError(
                            f"[Error] File: {file_name}, Sheet: {sheet_name}, Row {row_idx + 2}, Column {col_idx + 1} is missing data. "
                            "Field names and types cannot be empty."
                        )

            # 提取实际数据（从第四行开始）
            data_rows = df.iloc[3:].values.tolist()

            # 构建当前 sheet 的数据结构
            for row in data_rows:
                entry = {}
                for i, value in enumerate(row):
                    field_name = field_names[i]
                    field_type = field_types[i]

                    # 根据字段类型解析数据
                    if field_type == "int":
                        entry[field_name] = int(value) if pd.notnull(value) else 0
                    elif field_type == "string":
                        entry[field_name] = str(value) if pd.notnull(value) else ""
                    elif field_type == "float":
                        entry[field_name] = float(value) if pd.notnull(value) else 0
                    elif field_type == "bool":
                        entry[field_name] = bool(value) if pd.notnull(value) else False
                    elif field_type == "intTable":
                        entry[field_name] = parse_int_table(value)
                    elif field_type == "intArray":
                        entry[field_name] = parse_int_array(value)
                    elif field_type == "floatTable":
                        entry[field_name] = parse_float_table(value)
                    elif field_type == "floatArray":
                        entry[field_name] = parse_float_array(value)
                    elif field_type == "boolTable":
                        entry[field_name] = parse_bool_table(value)
                    elif field_type == "boolArray":
                        entry[field_name] = parse_bool_array(value)
                    elif field_type == "stringTable":
                        entry[field_name] = parse_string_table(value)
                    elif field_type == "stringArray":
                        entry[field_name] = parse_string_array(value)

                # 使用主键（Id）作为键
                entry_id = entry["Id"]
                if entry_id in result[base_file_name]:
                    raise ValueError(
                        f"[Error] Duplicate Id found: {entry_id} in File: {file_name}, Sheet: {sheet_name}. "
                        "Ids must be unique across all sheets."
                    )
                result[base_file_name][entry_id] = entry

    # 写入 JSON 文件
    with open(output_file, "w", encoding="utf-8") as f:
        json.dump(result, f, ensure_ascii=False, indent=4)
    print(f"JSON file generated successfully: {output_file}")


def parse_int_table(value):
    """解析 intTable 类型数据"""
    if pd.isnull(value) or str(value).strip() == "":
        return []
    pairs = str(value).split("|")
    return [[int(x.split("=")[0]), int(x.split("=")[1])] for x in pairs if "=" in x]


def parse_int_array(value):
    """解析 intArray 类型数据"""
    if pd.isnull(value) or str(value).strip() == "":
        return []
    return [int(x) for x in str(value).split("=") if x.strip() != ""]


def parse_float_table(value):
    """解析 floatTable 类型数据"""
    if pd.isnull(value) or str(value).strip() == "":
        return []
    pairs = str(value).split("|")
    return [[float(x.split("=")[0]), float(x.split("=")[1])] for x in pairs if "=" in x]


def parse_float_array(value):
    """解析 floatArray 类型数据"""
    if pd.isnull(value) or str(value).strip() == "":
        return []
    return [float(x) for x in str(value).split("=") if x.strip() != ""]


def parse_bool_table(value):
    """解析 boolTable 类型数据"""
    if pd.isnull(value) or str(value).strip() == "":
        return []
    pairs = str(value).split("|")
    return [[x.split("=")[0], bool(int(x.split("=")[1]))] for x in pairs if "=" in x]


def parse_bool_array(value):
    """解析 boolArray 类型数据"""
    if pd.isnull(value) or str(value).strip() == "":
        return []
    return [bool(int(x)) for x in str(value).split("=") if x.strip() != ""]


def parse_string_table(value):
    """解析 stringTable 类型数据"""
    if pd.isnull(value) or str(value).strip() == "":
        return []
    pairs = str(value).split("|")
    return [[x.split("=")[0], x.split("=")[1]] for x in pairs if "=" in x]


def parse_string_array(value):
    """解析 stringArray 类型数据"""
    if pd.isnull(value) or str(value).strip() == "":
        return []
    return [x for x in str(value).split("=") if x.strip() != ""]


if __name__ == "__main__":
    directory_path = "../"  # 输入目录路径
    generate_json(directory_path)
    input("输入任意键结束")