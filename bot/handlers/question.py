from telegram import Update
from telegram.ext import ContextTypes, ConversationHandler

from bot.config import ADMIN_CHAT_ID
from bot.keyboards.menu import get_main_menu_keyboard

WAITING_FOR_QUESTION = 1


async def ask_volunteer_start(update: Update, context: ContextTypes.DEFAULT_TYPE) -> int:
    query = update.callback_query
    await query.answer()

    await query.edit_message_text(
        text=(
            "🙋 Задать вопрос волонтёру\n\n"
            "Напишите ваше сообщение следующим сообщением в чат.\n\n"
            "Постарайтесь кратко описать ситуацию:\n"
            "- в чём проблема\n"
            "- какой у вас статус\n"
            "- что вы уже делали\n\n"
            "Чтобы отменить, напишите /cancel"
        )
    )
    return WAITING_FOR_QUESTION


async def receive_question(update: Update, context: ContextTypes.DEFAULT_TYPE) -> int:
    user = update.effective_user
    message_text = update.message.text

    username_line = f"📎 Username: @{user.username}\n" if user.username else ""

    forwarded_text = (
        "📨 Новый вопрос волонтёру\n\n"
        f"👤 Имя: {user.full_name}\n"
        f"🆔 User ID: {user.id}\n"
        f"{username_line}"
        f"\n💬 Сообщение:\n{message_text}"
    )

    await context.bot.send_message(
        chat_id=ADMIN_CHAT_ID,
        text=forwarded_text,
    )

    await update.message.reply_text(
        text=(
            "✅ Ваш вопрос отправлен волонтёру.\n\n"
            "Ожидайте ответа."
        ),
        reply_markup=get_main_menu_keyboard(),
    )

    return ConversationHandler.END


async def cancel_question(update: Update, context: ContextTypes.DEFAULT_TYPE) -> int:
    await update.message.reply_text(
        text="❌ Отправка вопроса отменена.",
        reply_markup=get_main_menu_keyboard(),
    )
    return ConversationHandler.END