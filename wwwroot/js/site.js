// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


const addRoleBtn = document.querySelector("#add-role-btn");
const addUserRoleBtn = document.querySelector("#add-user-role-btn");
const addRolePanel = document.querySelector("#new-role-panel");
const newUserRolePanel = document.querySelector("#new-user-role-panel");


if (addRoleBtn != null)
    addRoleBtn.addEventListener('click', () => {
        addRolePanel.classList.remove("hide");
        addRolePanel.classList.add("show");
    });

if (addUserRoleBtn != null)
    addUserRoleBtn.addEventListener('click', () => {
        newUserRolePanel.classList.toggle("hidden");
    });

