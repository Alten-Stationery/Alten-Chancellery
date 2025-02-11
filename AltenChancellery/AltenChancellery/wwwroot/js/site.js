const hamBurger = document.querySelector(".toggle-btn");
var subMenu = 0;

hamBurger.addEventListener("click", function () {
    document.querySelector("#sidebar").classList.toggle("expand");
    subMenu = 1;
});

