from telegram import Update
from telegram.ext import ContextTypes

from bot.config import ADMIN_CHAT_ID


async def reply_command(update: Update, context: ContextTypes.DEFAULT_TYPE) -> None:
    if update.effective_chat.id != ADMIN_CHAT_ID:
        await update.message.reply_text("У вас нет доступа к этой команде.")
        return

    args = context.args

    if len(args) < 2:
        await update.message.reply_text(
            "Использование:\n/reply user_id текст_ответа"
        )
        return

    try:
        user_id = int(args[0])
    except ValueError:
        await update.message.reply_text("user_id должен быть числом.")
        return

    reply_text = " ".join(args[1:])

    try:
        await context.bot.send_message(
            chat_id=user_id,
            text=(
                "🙋 Ответ от волонтёра\n\n"
                f"{reply_text}"
            ),
        )
        await update.message.reply_text("✅ Ответ отправлен пользователю.")
    except Exception as e:
        await update.message.reply_text(
            f"❌ Не удалось отправить сообщение.\nОшибка: {e}"
        )
