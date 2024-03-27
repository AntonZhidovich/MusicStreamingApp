import { ReleaseInSongModel } from "./ReleaseInSongModel";

export class SongModel {
    id = "";
    title = "";
    genres : string[] = [];
    release = new ReleaseInSongModel();
    durationMinutes = "";
    sourceName = "";
}