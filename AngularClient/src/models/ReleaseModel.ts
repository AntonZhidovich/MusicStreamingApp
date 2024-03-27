import { SongInReleaseModel } from "./SongInReleaseModel";

export class ReleaseModel {
    id = "";
    name = "";
    type = "";
    releasedAt = new Date();
    authorNames: string[] = [];
    songs: SongInReleaseModel[] = [];
    songsCount = 0;
    durationMinutes = "";
}