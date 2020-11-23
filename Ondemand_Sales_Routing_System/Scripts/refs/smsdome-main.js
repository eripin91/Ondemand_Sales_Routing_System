//*************************************************************
// Calculate SMS char length and parts
//*************************************************************
function textCounter(messagefield, counterfield) {
    var content = messagefield.value;
    var char_count = 0;
    var message_parts = 0;
    var unicode_count = 0;

    // Standardize line feed to /r/n
    content = content.replace(/\r\n/gm, '\n').replace(/[\r\n]/gm, '\r\n');
    unicode_count = getUnicodeCount(content);

    if (unicode_count == 0) {
        char_count = getSingleCharCount(content) + (getDoubleCharCount(content) * 2);
        if (char_count > 0)
            message_parts = (char_count <= 160) ? 1 : getMessageParts(char_count, 153);

    }
    else {
        char_count = content.length;
        if (char_count > 0)
            message_parts = (char_count <= 70) ? 1 : getMessageParts(char_count, 67);

    }


    counterfield.value = ((unicode_count == 0) ? "Standard SMS: " : "Unicode SMS: ") + char_count + ((char_count > 1) ? " chars (" : " char (") + message_parts + ((message_parts > 1) ? " credits)" : " credit)");

    return true;
}

function getUnicodeCount(content) {
    // Escape for /  \ [ ]
    // @£$¥èéùìòÇ\nØø\rÅåΔ_ΦΓΛΩΠΨΣΘΞÆæßÉ !""#¤%&'()*+,-./0-9:;<=>?¡A-ZÄÖÑÜ§¿a-zäöñüà\f^{}\[~]|€
    var unicode_regex = /[^@£$¥èéùìòÇ\nØø\rÅåΔ_ΦΓΛΩΠΨΣΘΞÆæßÉ !"#¤%&'()*+,-.\/0-9:;<=>?¡A-ZÄÖÑÜ§¿a-zäöñüà\f^{}\\\[~\]|€]/gm;
    var unicode_matches = content.match(unicode_regex)
    return unicode_matches ? unicode_matches.length : 0;
}

function getSingleCharCount(content) {
    // Escape for /  \ [ ]
    // @£$¥èéùìòÇ\nØø\rÅåΔ_ΦΓΛΩΠΨΣΘΞÆæßÉ !""#¤%&'()*+,-./0-9:;<=>?¡A-ZÄÖÑÜ§¿a-zäöñüà\f^{}\[~]|€
    var single_regex = /[^\f^{}\\\[~\]|€]/gm;
    var single_matches = content.match(single_regex)
    return single_matches ? single_matches.length : 0;
}

function getDoubleCharCount(content) {
    // Escape for /  \ [ ]
    // @£$¥èéùìòÇ\nØø\rÅåΔ_ΦΓΛΩΠΨΣΘΞÆæßÉ !""#¤%&'()*+,-./0-9:;<=>?¡A-ZÄÖÑÜ§¿a-zäöñüà\f^{}\[~]|€
    var double_regex = /[\f^{}\\\[~\]|€]/gm;
    var double_matches = content.match(double_regex)
    return double_matches ? double_matches.length : 0;
}

function getLineFeedCount(content) {
    // Escape for /  \ [ ]
    // @£$¥èéùìòÇ\nØø\rÅåΔ_ΦΓΛΩΠΨΣΘΞÆæßÉ !""#¤%&'()*+,-./0-9:;<=>?¡A-ZÄÖÑÜ§¿a-zäöñüà\f^{}\[~]|€
    var linefeed_regex = /[^\r]\n/gm;
    var linefeed_matches = content.match(linefeed_regex)
    return linefeed_matches ? linefeed_matches.length : 0;
}

function getCounterDisplay(content) {
    var unicode_count = 0;
    var single_count = 0;
    var double_count = 0;
    var char_count = 0;
    var message_parts = 0;

    // Standardize line feed to /r/n
    content = content.replace(/\r\n/gm, '\n').replace(/[\r\n]/gm, '\r\n');

    // Escape for /  \ [ ]
    // @£$¥èéùìòÇ\nØø\rÅåΔ_ΦΓΛΩΠΨΣΘΞÆæßÉ !""#¤%&'()*+,-./0-9:;<=>?¡A-ZÄÖÑÜ§¿a-zäöñüà\f^{}\[~]|€
    var unicode_regex = /[^@£$¥èéùìòÇ\nØø\rÅåΔ_ΦΓΛΩΠΨΣΘΞÆæßÉ !"#¤%&'()*+,-.\/0-9:;<=>?¡A-ZÄÖÑÜ§¿a-zäöñüà\f^{}\\\[~\]|€]/gm;
    var single_regex = /[^\f^{}\\\[~\]|€]/gm;
    var double_regex = /[\f^{}\\\[~\]|€]/gm;

    var unicode_matches = content.match(unicode_regex)
    unicode_count = unicode_matches ? unicode_matches.length : 0;

    if (unicode_count == 0) {
        var single_matches = content.match(single_regex)
        single_count = single_matches ? single_matches.length : 0;

        var double_matches = content.match(double_regex)
        double_count = double_matches ? double_matches.length : 0;

        char_count = single_count + (double_count * 2);
        if (char_count > 0)
            message_parts = (char_count <= 160) ? 1 : getMessageParts(char_count, 153);
    }
    else {
        char_count = content.length;
        if (char_count > 0)
            message_parts = (char_count <= 70) ? 1 : getMessageParts(char_count, 67);
    }

    return ((unicode_count == 0) ? "Standard SMS: " : "Unicode SMS: ") + char_count + ((char_count > 1) ? " chars (" : " char (") + message_parts + ((message_parts > 1) ? " credits)" : " credit)");
}

function getMessageParts(MessageCount, PartLength) {
    var parts = 0;

    parts = Math.floor(MessageCount / PartLength);
    if (MessageCount % PartLength != 0)
        parts = parts + 1;

    return parts
}