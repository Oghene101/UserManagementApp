const pageNumberElt = document.getElementById("page-number");
const prevPage = document.getElementById("prev-page");
const nextPage = document.getElementById("next-page");
const tableBody = document.querySelector(".display-table tbody");


nextPage.addEventListener("click", async () => {
    console.log("nxt page clicked.");
    var pageNumber = Number(pageNumberElt.textContent) + 1;
    getUsers(pageNumber)
        .then(data => PopulateTable(data));
    pageNumberElt.textContent = pageNumber;
    console.log("page" + pageNumberElt.textContent);

});

prevPage.addEventListener("click", () => {
    console.log("prev page clicked.");
    pageNumberElt.textContent--;
    //    currentPageRows = paginateTable(sheet_data.slice(1));
    //    populateTable(currentPageRows, headers);
});

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
        console.log(jsonData);
        return jsonData;
    } catch (error) {
        console.table(error);
    }
}

function PopulateTable(data) {
    let tableRows = "";
    data.forEach(user => {
        const getUserById = `${appBaseUrl}User/${user.id}`;
        tableRows +=
            `<tr>
                <td></td>
                <td>${user.firstName} ${user.lastName}</td>
                <td>${user.email}</td>
                <td class="flex-btw">
                    <a href="${getUserById}">View </a>
                    <a href="">Edit </a>
                    <form method="post" asp-action="DeleteUser" asp-controller="Dashboard" asp-route-userId="@user.Id">
                        <button style="border:none; outline:none; cursor:pointer; color:skyblue;"
                                onclick="return confirm('do you want to continue?')" type="submit">
                            Delete
                        </button>
                    </form>
                </td>
            </tr>`;
    });
    tableBody.innerHTML = tableRows;
}

