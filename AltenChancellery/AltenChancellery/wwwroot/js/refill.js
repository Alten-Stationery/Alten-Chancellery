// DOMContentLoaded = assicura che il codice viene eseguito quando il DOM è completamente caricato

document.addEventListener("DOMContentLoaded", function () {
    const lists = document.querySelectorAll(".list-group");
    let activeList = null;

    lists.forEach(list => {
        list.addEventListener("click", function (event) {
            if (event.target.classList.contains("list-group-item")) {
                if (activeList && activeList !== list) {
                    activeList.querySelectorAll(".list-group-item").forEach(item => {
                        item.classList.remove("active");
                    });
                }
                activeList = list;
                event.target.classList.toggle("active");
            }
        });
    });

    document.querySelector(".btn-secondary").addEventListener("click", function () {
        if (activeList) {
            activeList.querySelectorAll(".list-group-item").forEach(item => {
                item.classList.remove("active");
            });
        }
        activeList = null;
    });
});