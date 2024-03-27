import { Component } from "@angular/core";
import { ReleaseService } from "../../services/release.service";
import { ReleaseModel } from "../../models/ReleaseModel";
import { ReleaseItemComponent } from "../release-item/release-item.component";

@Component({
    selector: "releases",
    standalone: true,
    templateUrl: "./releases.component.html",
    imports: [ReleaseItemComponent],
    providers: [ReleaseService]
})
export class ReleasesComponent {

    releases: ReleaseModel[] = [];

    constructor (private releaseService: ReleaseService) {}
    
    ngOnInit() {
        this.releaseService.getReleases(1, 20)
            .subscribe(({
                next: (result) => {
                    this.releases = result.items;
                },
                error: (error) => {
                    console.log(error);
                }
            }));
    }
}