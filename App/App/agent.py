import os
import re
import sys

from google import genai

ROOT_DIR = os.getcwd()
MODEL_NAME = os.getenv("GEMINI_MODEL", "gemini-3-flash-preview")

SYSTEM_PROMPT = f"""
Ты — автономный исполнительный агент. Твоя рабочая директория: {ROOT_DIR}.
Твоя задача: писать код, который полностью решает запрос пользователя.
Правила:
1. Пиши только чистый код. Никаких объяснений.
2. Код должен быть готов к выполнению через exec().
3. Ты можешь создавать файлы, папки, слать запросы, менять конфиги.
4. Если нужны библиотеки, импортируй их (они должны быть стандартными или популярными).
5. Весь вывод (print) должен быть информативным, чтобы пользователь видел прогресс.
"""

client = genai.Client()


def extract_code(text: str) -> str:
    match = re.search(r"```python\s*(.*?)```", text, re.DOTALL | re.IGNORECASE)
    if match:
        return match.group(1).strip()
    return text.strip()


def run_agent(user_request: str):
    print(f"[*] Анализирую запрос: {user_request}")

    prompt = f"{SYSTEM_PROMPT}\n\nЗАПРОС ПОЛЬЗОВАТЕЛЯ: {user_request}\n\nТвой код:"

    try:
        response = client.models.generate_content(
            model=MODEL_NAME,
            contents=prompt,
        )

        raw_code = response.text or ""
        clean_code = extract_code(raw_code)

        print("-" * 30)
        print("[!] ВЫПОЛНЯЮ СЛЕДУЮЩИЙ КОД:")
        print(clean_code)
        print("-" * 30)

        exec_globals = {
            "__name__": "__main__",
            "os": os,
            "sys": sys,
        }
        exec(clean_code, exec_globals)
        print("[+] Задача выполнена успешно.")

    except Exception as e:
        print(f"[!] ОШИБКА: {e}")


if __name__ == "__main__":
    if len(sys.argv) < 2:
        request = input("Что нужно сделать? (или вставь путь к файлу с ТЗ): ")
    else:
        request = sys.argv[1]

    if os.path.exists(request):
        with open(request, "r", encoding="utf-8") as f:
            request = f.read()

    run_agent(request)