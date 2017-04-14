//範例來源：http://lab.25sprout.com/validate/

// 以下是各個 RULE
// 基本上 pattern 就是 
// jQuery.validator.addMethod("規則名稱英文", function( value, element ) {
// 	var str = value;
// 	var result = false;
// 	if(驗證通過){
// 		result = true;
// 	} else {
// 		result = false;
// 	}
// 	return result;
// }, "驗證不通過的訊息");

jQuery.validator.addMethod("mobileTaiwan", function (value, element) {
    var str = value;
    var result = false;
    if (str.length > 0) {
        //是否只有數字;
        var patt_mobile = /^[\d]{1,}$/;
        result = patt_mobile.test(str);

        if (result) {
            //檢查前兩個字是否為 09
            //檢查前四個字是否為 8869
            var firstTwo = str.substr(0, 2);
            var firstFour = str.substr(0, 4);
            var afterFour = str.substr(4, str.length - 1);
            if (firstFour == '8869') {
                $(element).val('09' + afterFour);
                if (afterFour.length == 8) {
                    result = true;
                } else {
                    result = false;
                }
            } else if (firstTwo == '09') {
                if (str.length == 10) {
                    result = true;
                } else {
                    result = false;
                }
            } else {
                result = false;
            }
        }
    } else {
        result = true;
    }
    return result;
}, "手機號碼不符合格式，僅允許09開頭的10碼數字（不含符號）");


jQuery.validator.addMethod("chkPid", function (value, element) {
    var people_id = value.replace(/\s+/g, "");
    return (
		this.optional(element) || /^[A-Z]{1}[1-2]{1}[0-9]{8}$/.test(people_id)
	);
}, "身分證字號需為一個大寫英文字開頭，且接續一個 1 或 2 數字的10個字字串");


jQuery.validator.addMethod("taiwanBirth", function (value, element) {
    var str = value;
    var result = false;
    if (str.length > 0) {
        //是否只有數字;
        var patt_digits = /^[\d]{1,}$/;
        result = patt_digits.test(str);

        if (result) {
            if (str.length == 7) {
                var firstThree = parseInt(str.substr(0, 3));
                var middleTwo = parseInt(str.substr(3, 2));
                var lastTwo = parseInt(str.substr(5, 2));

                if (firstThree <= 104) {
                    var setY = firstThree + 1911;
                    var setM = middleTwo - 1;
                    var setD = lastTwo;

                    var testDate = new Date(setY, setM, setD);
                    if (testDate.getMonth() == setM && testDate.getDate() == setD) {
                        result = true;
                    } else {
                        result = false;
                    }
                } else {
                    result = false;
                }
            } if (str.length == 6) {
                var firstTwo = parseInt(str.substr(0, 2));
                var middleTwo = parseInt(str.substr(2, 2));
                var lastTwo = parseInt(str.substr(4, 2));

                var setY = firstTwo + 1911;
                var setM = middleTwo - 1;
                var setD = lastTwo;

                var testDate = new Date(setY, setM, setD);
                if (testDate.getMonth() == setM && testDate.getDate() == setD) {
                    result = true;
                } else {
                    result = false;
                }
            }
        }
    } else {
        result = true;
    }
    return result;
}, "生日不符合格式");

jQuery.validator.addMethod("phoneStyle", function (value, element) {
    var str = value;
    var result = false;

    if (str.length > 0) {
        var patt_phone = /^[\d\-\(\)\#]{1,}$/;
        result = patt_phone.test(str);
    } else {
        result = true;
    }

    return result;
}, "電話號碼不符合格式，僅接受數字、#-()等符號");

jQuery.validator.addMethod("checkpwdhard", function (value, element) {
    var str = value;
    var result = false;

    if (str.length > 0) {
        var patt = /^[a-zA-Z0-9]{6,20}$/;
        var result1 = patt.test(str);
        //先測試是否有英文
        var pattEN = /[a-zA-Z]{1,}/;
        result2 = pattEN.test(str);
        //先測試是否有數字
        var pattDigit = /[0-9]{1,}/;
        result3 = pattDigit.test(str);

        if (result1 == true && result2 == true && result3 == true) {
            result = true;
        } else {
            result = false;
        }
    } else {
        result = true;
    }
    return result;
}, "密碼為 6～20個字元的英文字母、數字混合，但不含空白鍵及標點符號。");


jQuery.validator.addMethod("checkname", function (value, element) {
    var str = value;
    var result = false;

    if (str.length > 0) {
        //先測試是否有中文
        var pattCH = /[\u4e00-\u9fa5]{1,}/;
        result1 = pattCH.test(str);
        //先測試是否有英文
        var pattEN = /[a-zA-Z]{1,}/;
        result2 = pattEN.test(str);
        //檢查先生小姐
        testa = str.search("先生");
        testb = str.search("小姐");

        //整段內容只接受 (英文或中文)或空白、dash、underline
        var pattSimbo = /^[\u4e00-\u9fa5a-zA-Z\-_\s]{1,}$/;
        result3 = pattSimbo.test(str);

        //整段內容是否有空白、dash、underline
        var pattHasSimbo = /[\-_\s]{1,}/;
        result4 = pattHasSimbo.test(str);

        if (result1 && result2) {
            // console.log('有中文也有英文');
            result = false;
        } else {
            if (result1) {
                // console.log('有中文');
                if (str.length >= 2) {
                    // console.log('至少兩個字');	
                    if (testa == -1 && testb == -1) {
                        // console.log('沒有先生也沒有小姐');
                        if (result3) {
                            // console.log('符號 合規則');
                            if (result4) {
                                result = false;
                            } else {
                                result = true;
                            }
                        } else {
                            // console.log('符號不合規則');
                            result = false;
                        }
                    } else {
                        // console.log('有先生或小姐');
                        result = false;
                    }
                } else {
                    result = false;
                }
            } else {
                // console.log('沒有中文');
                result = false;
                if (result2) {
                    // console.log('有英文');
                    if (str.length >= 3) {
                        if (result3) {
                            // console.log('符號 合規則');
                            result = true;
                        } else {
                            // console.log('符號不合規則');
                            result = false;
                        }
                    } else {
                        result = false;
                    }
                } else {
                    // console.log('沒有英文');
                    result = false;
                }
            }
        }
    } else {
        result = true;
    }
    return result;
}, "姓名可為中文（最少兩字）或英文（最少三字），英文請勿使用除了空白、底線、DASH以外的符號");

//以下修改自網路
jQuery.validator.addMethod("TWIDCheck", function (value, element, param) {
    var a = new Array('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'X', 'Y', 'W', 'Z', 'I', 'O');
    var b = new Array(1, 9, 8, 7, 6, 5, 4, 3, 2, 1);
    var c = new Array(2);
    var d;
    var e;
    var f;
    var g = 0;
    var h = /^[a-z](1|2)\d{8}$/i;
    if (value.search(h) == -1) {
        return false;
    }
    else {
        d = value.charAt(0).toUpperCase();
        f = value.charAt(9);
    }
    for (var i = 0; i < 26; i++) {
        if (d == a[i])//a==a
        {
            e = i + 10; //10
            c[0] = Math.floor(e / 10); //1
            c[1] = e - (c[0] * 10); //10-(1*10)
            break;
        }
    }
    for (var i = 0; i < b.length; i++) {
        if (i < 2) {
            g += c[i] * b[i];
        }
        else {
            g += parseInt(value.charAt(i - 1)) * b[i];
        }
    }
    if ((g % 10) == f) {
        return true;
    }
    if ((10 - (g % 10)) != f) {
        return false;
    }
    return true;
}, "請輸入有效的身份證字號!");
//
jQuery.validator.addMethod("taiwanPhone", function (phone_number, element) {
    phone_number = phone_number.replace(/\s+/g, "");
    return this.optional(element) || phone_number.length > 9 &&
    phone_number.match(/^(\d{2,4}-?)?\d{6,8}$/g);
}, "請輸入有效的電話號碼！如：04-23583123 或 0912-123456");
//