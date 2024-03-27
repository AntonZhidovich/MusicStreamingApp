import { SongModel } from "./SongModel";

export class PlaylistWithSongsModel {
    id = "";
    name = "";
    userId = "";
    createdAt = new Date();
    songs: SongModel[] = [];
}