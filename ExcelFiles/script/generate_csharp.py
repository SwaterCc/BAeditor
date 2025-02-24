import pandas as pd
import os

def generate_csharp_classes(directory_path, output_dir="../../Assets/Hono/Scripts/Battle/BattleFoundation/ConfigDataBase/Config"):
    """
    根据目录下的所有 Excel 文件生成 C# 类文件。
    :param directory_path: 包含 Excel 文件的目录路径
    :param output_dir: 输出的 C# 类文件目录
    """
    # 确保输出目录存在
    if not os.path.exists(output_dir):
        os.makedirs(output_dir)

    # 获取目录下所有的 Excel 文件
    excel_files = [
        f for f in os.listdir(directory_path)
        if f.endswith(".xlsx") and not f.startswith("~$")
    ]

    for file_name in excel_files:
        file_path = os.path.join(directory_path, file_name)
        print(f"Processing file: {file_path}")

        # 去掉文件后缀名作为表名前缀
        base_file_name = os.path.splitext(file_name)[0]

        # 读取 Excel 文件
        excel_data = pd.ExcelFile(file_path)

        for sheet_name in excel_data.sheet_names:
            df = excel_data.parse(sheet_name, header=None)  # 不自动设置列名

            # 提取前三行作为表头信息
            field_descriptions = df.iloc[0].tolist()  # 第一行：描述
            field_names = df.iloc[1].tolist()         # 第二行：字段名
            field_types = df.iloc[2].tolist()         # 第三行：字段类型

            # 生成类名
            table_class_name = f"{sheet_name}"
            row_class_name = f"{sheet_name}Row"

            # 生成 C# 类代码
            class_code = generate_table_class(table_class_name, row_class_name, field_names, field_types)
            row_code = generate_row_class(row_class_name, field_names, field_types, field_descriptions)

            # 写入文件
            with open(os.path.join(output_dir, f"{table_class_name}.cs"), "w", encoding="utf-8") as f:
                f.write(class_code)
            with open(os.path.join(output_dir, f"{row_class_name}.cs"), "w", encoding="utf-8") as f:
                f.write(row_code)

            print(f"Generated classes for sheet: {sheet_name}")


def generate_table_class(table_class_name, row_class_name, field_names, field_types):
    """
    生成 ConfigTable 子类代码。
    """
    template = f"""
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Hono.Scripts.Battle
{{
    public class {table_class_name} : ConfigTable
    {{
        private readonly Dictionary<int, {row_class_name}> _rows = new Dictionary<int, {row_class_name}>();

        // 解析 JSON 对象
        public override void ParseJsonObject(JObject tableJObject)
        {{
            foreach (var rowJObject in tableJObject.Properties())
            {{
                var row = new {row_class_name}();
                row.Parse(rowJObject.Value as JObject);
                _rows[row.Id] = row;
            }}
        }}

        // 获取所有行（泛型版本）
        public IEnumerable<{row_class_name}> GetAllRows()
        {{
            return _rows.Values;
        }}

        // 获取单行（基类版本）
        public override ConfigTableRow GetRowBase(int id)
        {{
            return _rows.ContainsKey(id) ? _rows[id] : null;
        }}

        // 获取单行（子类版本）
        public {row_class_name} GetRow(int id)
        {{
            return _rows.ContainsKey(id) ? _rows[id] : null;
        }}

        public bool TryGetRow(int id,out {row_class_name} row)
        {{
            return _rows.TryGetValue(id,out row);
        }}
    }}
}}
"""
    return template.strip()


def generate_row_class(row_class_name, field_names, field_types, field_descriptions):
    """
    生成 ConfigTableRow 子类代码。
    """
    properties = []
    parse_statements = []

    for field_name, field_type, field_desc in zip(field_names[1:], field_types[1:], field_descriptions[1:]):
        csharp_type = get_csharp_type(field_type)
        xml_comment = format_xml_comment(field_desc)
        property_declaration = f"{xml_comment}\npublic {csharp_type} {field_name} {{ get; private set; }}"
        properties.append(property_declaration)

        # 根据字段类型生成解析语句
        parse_statements.append(f'{field_name} = rowJsonObject["{field_name}"].ToObject<{csharp_type}>();')

    template = f"""
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
namespace Hono.Scripts.Battle
{{
    /// <summary>
    /// 自动生成的 {row_class_name} 类。
    /// </summary>
    public class {row_class_name} : ConfigTableRow
    {{
        {"\n\n".join(properties)}

        // 解析 JSON 对象
        public override void Parse(JObject rowJsonObject)
        {{
            Id = rowJsonObject["Id"].Value<int>();
            {"; ".join(parse_statements)}
        }}
    }}
}}
"""
    return template.strip()


def get_csharp_type(field_type):
    """
    将字段类型转换为 C# 类型。
    """
    type_mapping = {
        "int": "int",
        "string": "string",
        "float": "float",
        "bool": "bool",
        "intTable": "List<List<int>>",
        "intArray": "List<int>",
        "floatTable": "List<List<float>>",
        "floatArray": "List<float>",
        "boolTable": "List<List<bool>>",
        "boolArray": "List<bool>",
        "stringTable": "List<List<string>>",
        "stringArray": "List<string>"
    }
    return type_mapping.get(field_type, "object")


def format_xml_comment(description):
    """
    格式化字段描述为 XML 注释。
    """
    if not description or str(description).strip() == "":
        return ""
    lines = [line.strip() for line in str(description).split("\n")]
    comment_lines = ["/// <summary>"] + [f"/// {line}" for line in lines] + ["/// </summary>"]
    return "\n".join(comment_lines)

def is_value_type(field_type):
    """
    判断字段类型是否为值类型。
    """
    value_types = {"int", "float", "bool"}
    return field_type in value_types

if __name__ == "__main__":
    directory_path = "../"  # 输入目录路径
    generate_csharp_classes(directory_path)