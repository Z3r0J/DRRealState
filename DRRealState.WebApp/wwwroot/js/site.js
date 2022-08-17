// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const inputs = document.querySelectorAll(".input");


function addcl() {
	let parent = this.parentNode.parentNode;
	parent.classList.add("focus");
}

function remcl() {
	let parent = this.parentNode.parentNode;
	if (this.value == "") {
		parent.classList.remove("focus");
	}
}


inputs.forEach(input => {
	input.addEventListener("focus", addcl);
	input.addEventListener("blur", remcl);
});


var root = document.documentElement;
var eyef = document.getElementById('eyef');
var cx = document.getElementById("eyef").getAttribute("cx");
var cy = document.getElementById("eyef").getAttribute("cy");

document.addEventListener("mousemove", evt => {
    let x = evt.clientX / innerWidth;
    let y = evt.clientY / innerHeight;

    root.style.setProperty("--mouse-x", x);
    root.style.setProperty("--mouse-y", y);

    cx = 115 + 30 * x;
    cy = 50 + 30 * y;
    eyef.setAttribute("cx", cx);
    eyef.setAttribute("cy", cy);

});

document.addEventListener("touchmove", touchHandler => {
    let x = touchHandler.touches[0].clientX / innerWidth;
    let y = touchHandler.touches[0].clientY / innerHeight;

    root.style.setProperty("--mouse-x", x);
    root.style.setProperty("--mouse-y", y);
});