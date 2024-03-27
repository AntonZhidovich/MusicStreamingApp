export class AuthorModel {
    name = "";
    userNames: string[] = [];
    createdAt = new Date();
    isBroken  = false;
    brokenAt = new Date();
    description = "";
}