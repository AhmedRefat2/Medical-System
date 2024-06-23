function randomNum() {
    "use strict";
    return Math.floor(Math.random() * 9) + 1;
}
var loop1, loop2, loop3, time = 30, i = 0, number, selector3 = document.querySelector('.thirdDigit'), selector2 = document.querySelector('.secondDigit'),
    selector1 = document.querySelector('.firstDigit');
loop3 = setInterval(function () {
    "use strict";
    if (i > 40) {
        clearInterval(loop3);
        selector3.textContent = selector3.getAttribute("data-value");
    } else {
        selector3.textContent = randomNum();
        i++;
    }
}, time);
loop2 = setInterval(function () {
    "use strict";
    if (i > 80) {
        clearInterval(loop2);
        selector2.textContent = selector2.getAttribute("data-value");
    } else {
        selector2.textContent = randomNum();
        i++;
    }
}, time);
loop1 = setInterval(function () {
    "use strict";
    if (i > 100) {
        clearInterval(loop1);
        selector1.textContent = selector1.getAttribute("data-value");
    } else {
        selector1.textContent = randomNum();
        i++;
    }
}, time);


/// go back
const goBackBTN = document.querySelector("#go-back");
goBackBTN.addEventListener("click", e => {
    e.preventDefault(); // Prevent any default action
    window.history.back(); // Go back to the previous page
});
