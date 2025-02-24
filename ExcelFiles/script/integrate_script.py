import os
import sys

def get_executable_dir():
    """
    获取当前 .exe 文件所在的目录。
    如果是未打包的 Python 脚本，则返回脚本所在目录。
    """
    if getattr(sys, 'frozen', False):
        # 打包后的 .exe 环境
        return os.path.dirname(sys.executable)
    else:
        # 未打包的 Python 脚本环境
        return os.path.dirname(os.path.abspath(__file__))

def run_generate_json():
    """运行生成 JSON 的脚本"""
    script_path = os.path.join(get_executable_dir(), "generate_json.py")
    if not os.path.exists(script_path):
        raise FileNotFoundError(f"Script not found: {script_path}")
    os.system(f"python {script_path}")

def run_generate_csharp():
    """运行生成 C# 类的脚本"""
    script_path = os.path.join(get_executable_dir(), "generate_csharp.py")
    if not os.path.exists(script_path):
        raise FileNotFoundError(f"Script not found: {script_path}")
    os.system(f"python {script_path}")

if __name__ == "__main__":
    print("Generating JSON...")
    run_generate_json()

    print("Generating C# classes...")
    run_generate_csharp()

    import os
import sys