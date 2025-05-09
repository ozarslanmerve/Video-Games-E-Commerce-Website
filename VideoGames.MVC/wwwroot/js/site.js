document.addEventListener("DOMContentLoaded", function () {
    const navbar = document.getElementById("navbarNav");
    const collapse = new bootstrap.Collapse(navbar, { toggle: false });
    const mainContent = document.getElementById("mainContent");

    navbar.addEventListener("show.bs.collapse", function () {
        mainContent.style.paddingTop = "400px";
    });

    navbar.addEventListener("hidden.bs.collapse", function () {
        mainContent.style.paddingTop = "110px";
    });
});
