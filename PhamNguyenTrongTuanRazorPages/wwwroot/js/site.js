"use strict";

const connection = new signalR.HubConnectionBuilder().withUrl("/signalrserver").build();

connection.on("LoadArticles", function (user, message) {
    LoadNewsArticle();
});

connection.start().then(function () {
    LoadNewsArticle();
}).catch(function (err) {
    return console.error(err.toString());
});



function LoadNewsArticle(
    sortOrder = "",
    searchString = "",
    pageNumber = 1,
    pageSize = 4) {

    
    $.ajax({
        type: 'GET',
        url: `/NewsArticle/Index?handler=AllNewsArticles&searchString=${searchString}&pageNumber=${pageNumber}&sortOrder=${sortOrder}&pageSize=${pageSize}`,
        success: function (data) {
            const totalPages = data.totalPages;
            const totalElements = data.totalElements;
            const pageIndex = data.pageIndex;
            const hasPreviousPage = data.hasPreviousPage;
            const hasNextPage = data.hasNextPage;
            const currentSort = data.currentSort;
            const titleSortParam = data.titleSortParam;
            const dateSortParam = data.dateSortParam;

            if (sortOrder == null) {
                sortOrder = "";
            }
            if (searchString == null) {
                searchString = "";
            }
            let tr = '';

            $.each(data.newsArticles, function (i, item) {
                let modifieddate = new Date(item.modifiedDate);
                let localDate = new Date(modifieddate.getTime() - modifieddate.getTimezoneOffset() * 60000);
                let formattedDate = localDate.toLocaleString('en-GB', { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' });
                tr += `
                    <tr>
                        <td>
                            ${item.newsArticleId}
                        </td>
                        <td>
                            ${item.newsTitle}
                        </td>
                        <td>
                            ${item.headline}
                        </td>
                        <td>
                            ${item.newsSource}
                        </td>
                        <td>
                            ${item.createdByName}
                        </td>
                        <td>
                            ${formattedDate}
                        </td>
                        <td>
                            ${item.updatedByName}
                        </td>
                        <td>
                            ${item.categoryName}
                        </td>
                        <td>
                            ${item.newsStatus ? `<span class="badge badge-success">Active</span>` : `<span class="badge badge-danger">Inactive</span>`}
                        </td>
                        <td>
                            <a href="/NewsArticle/Edit?id=${item.newsArticleId}">Edit</a> |
                            <a href="/NewsArticle/Details?id=${item.newsArticleId}">Details</a> |
                            <a href="/NewsArticle/Delete?id=${item.newsArticleId}">Delete</a>
                        </td>
                    </tr>
                `;
            });
            $('#newsArticlesBody').html(tr);

            //Add search input
            let searchInput = `<input class="form-control mr-sm-2" type="text" placeholder="Search title or author" aria-label="Search" name="searchString" value="${searchString}" id="searchInput" />`
            $('#searchInputWrapper').html(searchInput);
            //Add sort title
            let title = `<a href="javascript:void(0)" id="titleSort" data-titlesortparam="${titleSortParam}">
                            Title
                        </a>`;
            $('#titleSortWrapper').html(title);

            //Add sort date
            let date = `<a href="javascript:void(0)" id="dateSort" data-datesortparam="${dateSortParam}">
                            Modified Date
                        </a>`;
            $('#dateSortWrapper').html(date);

            //Add to pagination
            let paginationLi = '';
            paginationLi = `<li class="page-item ${!hasPreviousPage ? "disabled" : ""}">
                                <a class="page-link"
                                   href="javascript:void(0)"
                                   aria-disabled="${!hasPreviousPage ? "true" : "false"}"
                                   data-sortorder="${currentSort}"
                                   data-pagenumber="${pageIndex - 1}">
                                    Previous
                                </a>
                            </li>`;

            for (let i = 1; i <= totalPages; i++) {
                paginationLi += `<li class="page-item ${i == pageIndex ? "active" : ""}">
                                <a class="page-link"
                                   href="javascript:void(0)"
                                   aria-current="${i == pageIndex ? "true" : "false"}"
                                   data-sortorder="${currentSort}"
                                   data-pagenumber="${i}">
                                    ${i}
                                </a>
                            </li>`;
            }

            paginationLi += `<li class="page-item ${!hasNextPage ? "disabled" : ""}">
                                <a class="page-link"
                                   href="javascript:void(0)"
                                   aria-disabled="${!hasNextPage ? "true" : "false"}"
                                   data-sortorder="${currentSort}"
                                   data-pagenumber="${pageIndex + 1}">
                                    Previous
                                </a>
                            </li>`;
            $('#paginationSection').html(paginationLi);

            
        },
        error: function (error) {
            console.log(error);
        }
    });
}

function submitSearchForm() {
    console.log("hello world");
}