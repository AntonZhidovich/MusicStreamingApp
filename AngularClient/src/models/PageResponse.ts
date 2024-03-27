import { AuthorModel } from "./AuthorModel"
import { ReleaseModel } from "./ReleaseModel"

export class PageResponse {
    currentPage = 0;
    pageSize = 0;
    pagesCount = 0;
    items: any[] = [];
}