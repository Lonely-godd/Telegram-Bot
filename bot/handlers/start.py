from telegram import Update
from telegram.ext import ContextTypes

from bot.keyboards.menu import get_main_menu_keyboard


async def start_command(update: Update, context: ContextTypes.DEFAULT_TYPE) -> None:
    text = (
        "🇺🇦 Добро пожаловать!\n\n"
        "Этот бот создан для помощи украинцам в Лионе "
        "по административным и бытовым вопросам.\n\n"
        "Здесь вы сможете найти ответы на частые вопросы, "
        "полезные контакты и информацию по важным процедурам.\n\n"
        "Выберите нужный раздел ниже:"
    )

    await update.message.reply_text(
        text=text,
        reply_markup=get_main_menu_keyboard(),
    )