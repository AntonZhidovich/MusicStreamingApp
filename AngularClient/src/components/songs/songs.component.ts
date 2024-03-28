import { Component, HostListener } from "@angular/core";
import { SongModel } from "../../models/SongModel";
import { SongService } from "../../services/song.service";
import { PlayerService } from "../../services/player.service";
import { InfiniteScrollDirective  } from "../../InfiniteScrollDirective";

@Component({
    selector: "songs",
    standalone: true,
    templateUrl: "./songs.component.html",
    imports: [InfiniteScrollDirective]
})
export class SongsComponent {
    songs: SongModel[] = [];
    pageInfo = {
        currentPage: 0,
        pageSize: 6,
        pagesCount: Number.MAX_VALUE
    }
    isLoadingPage = false;
    
    constructor(private songService: SongService,
                private playerService: PlayerService) {}

    ngOnInit() {
        this.loadNextPage();
    }

    onSongClick(songNumber: number) {
        this.playerService.setPlaylist(this.songs, songNumber);
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
        this.songService.getSongs(this.pageInfo.currentPage, this.pageInfo.pageSize)
            .subscribe({
                next: (result) => {
                    this.songs = this.songs.concat(result.items);
                    this.pageInfo.pagesCount = result.pagesCount;
                    this.isLoadingPage = false;
                },
                error: (error) => {
                    console.log(error);
                    this.isLoadingPage = false;
                }
        });
    }
}