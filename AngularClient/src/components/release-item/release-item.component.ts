import { Component, EventEmitter, Input, Output } from "@angular/core";
import { ReleaseService } from "../../services/release.service";
import { ReleaseModel } from "../../models/ReleaseModel";
import { DatePipe } from "@angular/common";

@Component({
    selector: "release-item",
    standalone: true,
    templateUrl: "release-item.component.html",
    providers: [ReleaseService],
    imports: [DatePipe]
})
export class ReleaseItemComponent{
    @Input() release = new ReleaseModel();
    @Output() onAuthorDeleted = new EventEmitter();

    constructor(private releaseService: ReleaseService) {}
    
    onDeleteClick(release: ReleaseModel) {
        if (!confirm(`You sure you want to remove release ${release.name}?`)){
            return;
        }

        this.releaseService.deleteRelease(release.id)
            .subscribe({
                next: (result) => {
                    this.onAuthorDeleted.emit();
                },
                error: (error) => {
                    console.log(error);
                }
            });
    }
}