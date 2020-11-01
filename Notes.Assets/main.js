import "highlight.js/styles/monokai.css";
import "./styles.scss";
import "codemirror/mode/markdown/markdown";
import "codemirror/mode/xml/xml";
import "codemirror/mode/javascript/javascript";
import "codemirror/mode/ruby/ruby";
import "codemirror/mode/python/python";
import "codemirror/mode/php/php";
import "codemirror/mode/yaml/yaml";
import "codemirror/mode/clike/clike";
import CodeMirror from "codemirror";
import $ from "cash-dom";
import hljs from "highlight.js";

hljs.initHighlightingOnLoad();
const codeArea = document.getElementById("code");

if (codeArea != null) {
    CodeMirror.fromTextArea(codeArea, {
        lineNumbers: true,
        mode: "markdown",
        theme: "monokai"
    });
}

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
