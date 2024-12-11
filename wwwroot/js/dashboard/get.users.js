const pageNumberElt = document.getElementById("page-number");
const prevPage = document.getElementById("prev-page");
const nextPage = document.getElementById("next-page");
const tableBody = document.querySelector(".users-table tbody");

let pageNumber = 1, pageSize = 10;

nextPage.addEventListener("click", async () => {
    console.log("nxt page clicked.");
    if (pageNumber < numberOfPages) {
        pageNumber++;
        loadPage();
    }
});

prevPage.addEventListener("click", () => {
    console.log("prev page clicked.");
    if (pageNumber > 1) {
        pageNumber--;
        loadPage();
    }
});

document.querySelectorAll(".delete-user").forEach((btn) => {
    btn.addEventListener("click", (event) => {
        Swal.fire({
            title: "Are you sure?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes, delete it!"
        }).then((result) => {
            if (result.isConfirmed) {
                const userId = event.target.getAttribute("data-id");
                deleteUserById(userId)
                    .then(() => loadPage())
                    .then(() => Swal.fire({
                        title: "Deleted!",
                        text: "Your file has been deleted.",
                        icon: "success"
                    }));
            }
        });

    });
});

function loadPage() {
    getUsers(pageNumber)
        .then(data => populateTable(data))
        .then(() => updatePaginationControls());
}

async function getUsers(pageNumber) {
    try {
        let response = await fetch(`${appBaseUrl}Dashboard/Users?PageNumber=${pageNumber}`, {
            headers: {
                "Accept": "application/json"
            }
        });

        if (!response.ok) {
            let errorResponse;

            try {
                errorResponse = await response.json();
            } catch (jsonError) {
                errorResponse = await response.text();
            }

            console.table(response);
            throw errorResponse;
        }

        let jsonData = await response.json();
        console.log({pageNumber, data: jsonData});
        return jsonData;
    } catch (error) {
        console.table(error);
    }
}

async function deleteUserById(userId) {
    try {
        console.log(`Delete btn clicked for ${userId}`);
        let response = await fetch(`${appBaseUrl}DeleteUser/${userId}`, {
            method: "DELETE",
            headers: {
                "Accept": "application/json"
            }
        });

        if (!response.ok) {
            let errorResponse;

            try {
                errorResponse = await response.json();
            } catch (jsonError) {
                errorResponse = await response.text();
            }

            console.table(response);
            throw errorResponse;
        }
        console.log("Deleted user successfully.");

    } catch (error) {
        if (error && error.errors && Array.isArray(error.errors) && error.errors.length > 0) {
            error.errors.forEach((error) => {
                toastr.error((error.message === "The specified value is null.") ? "User has been deleted." : error.message);
            });
        }
        console.table(error);
    }
}

function populateTable(data) {
    let tableRows = "";
    let startRowNumber = (pageNumber - 1) * pageSize + 1; //to avoid pagination resetting row numbering for each page, hence startRowNumber.

    data.pageItems.forEach((user, index) => {
        const rowNumber = startRowNumber + index;
        const getUserById = `${appBaseUrl}Dashboard/User/${user.id}`;
        tableRows +=
            `<tr>
                <td class="border border-gray-300 px-4 py-2">${rowNumber}</td>
                <td class="border border-gray-300 px-4 py-2">${user.firstName} ${user.lastName}</td>
                <td class="border border-gray-300 px-4 py-2">${user.email}</td>
                <td class="border border-gray-300 px-4 py-2">
                    <a class="text-blue-500 hover:underline" href="${getUserById}">View </a>
                    <a class="text-green-500 hover:underline" href="">Edit </a>
                    <button class="delete-user text-red-500 hover:underline"
                            type="button" data-id="${user.id}">
                        Delete
                    </button>
                </td>
            </tr>`;
    });
    tableBody.innerHTML = tableRows;

    //Re-bind delete buttons
    document.querySelectorAll(".delete-user").forEach((btn) => {
        btn.addEventListener("click", (event) => {
            Swal.fire({
                title: "Are you sure?",
                text: "You won't be able to revert this!",
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Yes, delete it!"
            }).then((result) => {
                if (result.isConfirmed) {
                    const userId = event.target.getAttribute("data-id");
                    deleteUserById(userId)
                        .then(() => loadPage())
                        .then(() => Swal.fire({
                            title: "Deleted!",
                            text: "Your file has been deleted.",
                            icon: "success"
                        }));
                }
            });
        });
    });
}

function updatePaginationControls() {
    pageNumberElt.textContent = pageNumber;
    prevPage.disabled = pageNumber === 1;
    nextPage.disabled = pageNumber === numberOfPages;
}

    