import { Component } from "@angular/core";
import { AuthorModel } from "../../models/AuthorModel";
import { AuthorService } from "../../services/author.service";
import { DatePipe } from "@angular/common";

@Component({
    selector: "authors",
    standalone: true,
    templateUrl: "./authors.component.html",
    imports: [DatePipe],
    providers: [AuthorService]
})
export class AuthorsComponent {
    
    authors: AuthorModel[] = [];
    currentPage = 1;
    pageSize = 10;
    pagesCount = 1;
    endOfList = false;

    constructor(private authorService: AuthorService) {}
    
    ngOnInit() {
        this.loadAuthors();
    }

    private loadAuthors() {
        if (this.endOfList) {
            return;
        }

        this.authorService.getAuthors(this.currentPage, this.pageSize)
        .subscribe({
            next: (result: any) => {
                this.pagesCount = result.pagesCount;
                this.currentPage = result.currentPage;
                this.authors = this.authors.concat(result.items);
            }
        })
    }
}