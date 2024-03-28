import { Component } from "@angular/core";
import { AuthorModel } from "../../models/AuthorModel";
import { AuthorService } from "../../services/author.service";
import { DatePipe } from "@angular/common";
import { InfiniteScrollDirective } from "../../InfiniteScrollDirective";

@Component({
    selector: "authors",
    standalone: true,
    templateUrl: "./authors.component.html",
    imports: [InfiniteScrollDirective, DatePipe],
    providers: [AuthorService]
})
export class AuthorsComponent {
    
    authors: AuthorModel[] = [];

    pageInfo = {
        currentPage: 0,
        pageSize: 10,
        pagesCount: Number.MAX_VALUE
    }
    isLoadingPage = false;

    constructor(private authorService: AuthorService) {}
    
    ngOnInit() {
        this.loadNextPage();
    }

    onLoadElements() {
        if (!this.isLoadingPage){
            if (this.pageInfo.currentPage < this.pageInfo.pagesCount) {
                this.loadNextPage();
            }
        }
    }

    private loadNextPage() {
        ++this.pageInfo.currentPage;
        this.isLoadingPage = true;
        this.authorService.getAuthors(this.pageInfo.currentPage, this.pageInfo.pageSize)
        .subscribe({
            next: (result: any) => {
                this.pageInfo.pagesCount = result.pagesCount;
                this.authors = this.authors.concat(result.items);
                this.isLoadingPage = false;
            },
            error: (error) => {
                console.log(error);
                this.isLoadingPage = false;
            }
        })
    }
}