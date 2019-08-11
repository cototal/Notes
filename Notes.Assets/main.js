import "./styles.scss";
import $ from "cash-dom";


$(() => {
    // Bulma navbar
    const $navbarBurgers = $(".navbar-burger");
    if ($navbarBurgers.length > 0) {
        $navbarBurgers.each((idx, el) => {
            const $el = $(el);
            $el.on("click", () => {
                const $target = $(`#${$el.data("target")}`);
                $target.toggleClass("is-active");
            });
        });
    }
});
