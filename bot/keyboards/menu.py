from telegram import InlineKeyboardButton, InlineKeyboardMarkup


def get_main_menu_keyboard() -> InlineKeyboardMarkup:
    keyboard = [
        [InlineKeyboardButton("❓ Часто задаваемые вопросы", callback_data="faq_menu")],
        [InlineKeyboardButton("📅 Взять рандеву с ADN", callback_data="adn")],
        [InlineKeyboardButton("📍 Полезные адреса и контакты", callback_data="contacts")],
        [InlineKeyboardButton("🙋 Задать вопрос волонтёру", callback_data="ask_volunteer")],
        [InlineKeyboardButton("ℹ️ О боте", callback_data="about")],
    ]
    return InlineKeyboardMarkup(keyboard)


def get_faq_menu_keyboard() -> InlineKeyboardMarkup:
    keyboard = [
        [InlineKeyboardButton("🏛 Префектура", callback_data="faq_prefecture")],
        [InlineKeyboardButton("🛂 Первая подача на временную защиту", callback_data="faq_first_tp")],
        [InlineKeyboardButton("🔄 Продление временной защиты", callback_data="faq_renew_tp")],
        [InlineKeyboardButton("📦 CAF", callback_data="faq_caf")],
        [InlineKeyboardButton("💼 France Travail", callback_data="faq_france_travail")],
        [InlineKeyboardButton("🏠 Жильё", callback_data="faq_housing")],
        [InlineKeyboardButton("🏥 Медицина", callback_data="faq_health")],
        [InlineKeyboardButton("🧾 OFII / ADA", callback_data="faq_ofii_ada")],
        [InlineKeyboardButton("🛡 Asile", callback_data="faq_asile")],
        [InlineKeyboardButton("📚 Школа / курсы французского", callback_data="faq_school")],
        [InlineKeyboardButton("⬅️ Назад в главное меню", callback_data="back_main")],
    ]
    return InlineKeyboardMarkup(keyboard)


def get_back_to_faq_keyboard() -> InlineKeyboardMarkup:
    keyboard = [
        [InlineKeyboardButton("⬅️ Назад к FAQ", callback_data="faq_menu")],
        [InlineKeyboardButton("🏠 Главное меню", callback_data="back_main")],
    ]
    return InlineKeyboardMarkup(keyboard)


def get_cancel_question_keyboard() -> InlineKeyboardMarkup:
    keyboard = [
        [InlineKeyboardButton("❌ Отмена", callback_data="cancel_question")],
    ]
    return InlineKeyboardMarkup(keyboard)


def get_language_keyboard() -> InlineKeyboardMarkup:
    keyboard = [
        [InlineKeyboardButton("Русский", callback_data = "set_lang_ru")],
        [InlineKeyboardButton("Українська", callback_data = "set_lang_ua")],
    ]
    return InlineKeyboardMarkup(keyboard)