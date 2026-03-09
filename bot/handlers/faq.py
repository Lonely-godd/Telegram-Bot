from telegram import Update
from telegram.ext import ContextTypes

from bot.keyboards.menu import (
    get_main_menu_keyboard,
    get_faq_menu_keyboard,
    get_back_to_faq_keyboard,
)


FAQ_TEXTS = {
    "faq_prefecture": (
        "🏛 Префектура\n\n"
        "Здесь будет информация о префектуре:\n"
        "- как записаться\n"
        "- что делать, если нет письма\n"
        "- какие документы брать\n"
        "- как проходит продление\n\n"
        "Пока это временная заглушка."
    ),
    "faq_first_tp": (
        "🛂 Первая подача на временную защиту\n\n"
        "Здесь будет информация о первой подаче:\n"
        "- куда обращаться\n"
        "- какие документы подготовить\n"
        "- как проходит процедура\n\n"
        "Пока это временная заглушка."
    ),
    "faq_renew_tp": (
        "🔄 Продление временной защиты\n\n"
        "Здесь будет информация о продлении:\n"
        "- когда начинать\n"
        "- какие документы нужны\n"
        "- что делать при задержке\n\n"
        "Пока это временная заглушка."
    ),
    "faq_caf": (
        "📦 CAF\n\n"
        "Здесь будет информация по CAF:\n"
        "- как создать аккаунт\n"
        "- как подать dossier\n"
        "- что делать, если выплаты не приходят\n\n"
        "Пока это временная заглушка."
    ),
    "faq_france_travail": (
        "💼 France Travail\n\n"
        "Здесь будет информация по France Travail:\n"
        "- как зарегистрироваться\n"
        "- какие документы нужны\n"
        "- помощь с поиском работы\n\n"
        "Пока это временная заглушка."
    ),
    "faq_housing": (
        "🏠 Жильё\n\n"
        "Здесь будет информация по жилью:\n"
        "- где искать\n"
        "- какие есть формы помощи\n"
        "- что делать при срочной ситуации\n\n"
        "Пока это временная заглушка."
    ),
    "faq_health": (
        "🏥 Медицина\n\n"
        "Здесь будет информация по медицине:\n"
        "- CPAM\n"
        "- carte vitale\n"
        "- как записаться к врачу\n\n"
        "Пока это временная заглушка."
    ),
    "faq_ofii_ada": (
        "🧾 OFII / ADA\n\n"
        "Здесь будет информация по OFII и ADA:\n"
        "- что это такое\n"
        "- кто имеет право\n"
        "- как решать проблемы с выплатами\n\n"
        "Пока это временная заглушка."
    ),
    "faq_asile": (
        "🛡 Asile\n\n"
        "Здесь будет информация по запросу убежища:\n"
        "- базовые шаги\n"
        "- различие между asile и protection temporaire\n"
        "- куда обращаться\n\n"
        "Пока это временная заглушка."
    ),
    "faq_school": (
        "📚 Школа / курсы французского\n\n"
        "Здесь будет информация:\n"
        "- школа для детей\n"
        "- языковые курсы\n"
        "- интеграционные программы\n\n"
        "Пока это временная заглушка."
    ),
}


async def menu_callback(update: Update, context: ContextTypes.DEFAULT_TYPE) -> None:
    query = update.callback_query
    await query.answer()

    data = query.data

    if data == "faq_menu":
        await query.edit_message_text(
            text=(
                "❓ Часто задаваемые вопросы\n\n"
                "Выберите нужную тему:"
            ),
            reply_markup=get_faq_menu_keyboard(),
        )
        return

    if data == "back_main":
        await query.edit_message_text(
            text=(
                "🇺🇦 Главное меню\n\n"
                "Выберите нужный раздел:"
            ),
            reply_markup=get_main_menu_keyboard(),
        )
        return

    if data == "adn":
        await query.edit_message_text(
            text=(
                "📅 Взять рандеву с ADN\n\n"
                "Здесь позже будет логика записи или инструкции "
                "по получению рандеву с ADN.\n\n"
                "Пока это временная заглушка."
            ),
            reply_markup=get_main_menu_keyboard(),
        )
        return

    if data == "contacts":
        await query.edit_message_text(
            text=(
                "📍 Полезные адреса и контакты\n\n"
                "Здесь позже будут адреса и контакты:\n"
                "- ADN\n"
                "- Префектура\n"
                "- CAF\n"
                "- France Travail\n"
                "- CPAM\n\n"
                "Пока это временная заглушка."
            ),
            reply_markup=get_main_menu_keyboard(),
        )
        return

    if data == "ask_question":
        await query.edit_message_text(
            text=(
                "📝 Написать свой вопрос\n\n"
                "Здесь позже будет форма, через которую пользователь "
                "сможет отправить свой вопрос.\n\n"
                "Пока это временная заглушка."
            ),
            reply_markup=get_main_menu_keyboard(),
        )
        return

    if data == "about":
        await query.edit_message_text(
            text=(
                "ℹ️ О боте\n\n"
                "Этот бот создан для помощи украинским беженцам в Лионе.\n\n"
                "Важно: информация в боте носит справочный характер "
                "и может обновляться."
            ),
            reply_markup=get_main_menu_keyboard(),
        )
        return

    if data in FAQ_TEXTS:
        await query.edit_message_text(
            text=FAQ_TEXTS[data],
            reply_markup=get_back_to_faq_keyboard(),
        )
        return