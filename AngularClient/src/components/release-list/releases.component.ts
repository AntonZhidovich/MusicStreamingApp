import { Component } from "@angular/core";
import { ReleaseService } from "../../services/release.service";
import { ReleaseModel } from "../../models/ReleaseModel";
import { ReleaseItemComponent } from "../release-item/release-item.component";
import { InfiniteScrollDirective } from "../../InfiniteScrollDirective";

@Component({
    selector: "releases",
    standalone: true,
    templateUrl: "./releases.component.html",
    imports: [InfiniteScrollDirective, ReleaseItemComponent],
    providers: [ReleaseService]
})
export class ReleasesComponent {

    releases: ReleaseModel[] = [];
    pageInfo = {
        currentPage: 0,
        pageSize: 8,
        pagesCount: Number.MAX_VALUE
    }
    isLoadingPage = false;

    constructor (private releaseService: ReleaseService) {}
    
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

    loadNextPage() {
        ++this.pageInfo.currentPage;
        this.isLoadingPage = true;
        this.releaseService.getReleases(this.pageInfo.currentPage, this.pageInfo.pageSize)
        .subscribe(({
            next: (result) => {
                this.releases = this.releases.concat(result.items);
                this.pageInfo.pagesCount = result.pagesCount;
                this.isLoadingPage = false;
            },
            error: (error) => {
                console.log(error);
                this.isLoadingPage = false;
            }
        }));
    }
}