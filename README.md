# SPG Messenger

This project, the Cool-Messenger, is a little messaging platform designed to communicate through text and media messages.
It has a domain model for handling users, messengerEntries, messages, and relationships.
It aims to mimic the idea of Messengers we know from WhatsApp or Signal or platforms like Facebook.

## Team
- Sheikhi Hengvin

## Key Features

- User account management
- Creation and management of messenger entries
- Support for text and media messages
- Relationship management among users

## Sample Code

Here's an example of how to create a messenger entry and send messages:

```C#
var user1 = new User("email1@example.com", "password1", new Profile("John", "Doe"));
var user2 = new User("email2@example.com", "password2", new Profile("Jane", "Doe"));

var messengerEntry = new MessengerEntry(user1, new HashSet<User> { user1, user2 }, "Group Chat", "This is a group chat.");
messengerEntry.SendTextMessage(user1, "Hello, everyone!");

var media = new Media("image.jpg", "image/jpeg", 1024, 800, 600);
messengerEntry.SendMediaMessage(user2, "Check this out!", media);

@startuml

abstract class BaseEntity {
    + Id: int
    + Guid: Guid
    + CreatedAt: DateTime
}

class User {
    + Profile: Profile
    + Account: Account
    + Role: UserRole
    + Messengers: List<MessengerEntry>
    + Relations: List<Relation>
    + AddFriend(friend: User)
    + AcceptFriend(friend: User)
    + RemoveFriend(friend: User)
}

class Profile {
    + FirstName: string
    + LastName: string
}

class Account {
    + Verified: bool
    + Locked: bool
}

class MessengerEntry {
    + Title: string
    + Description: string
    + Participants: IReadOnlySet<User>
    + Messages: IReadOnlyList<Message>
    + AddParticipant(newParticipant: User)
    + RemoveParticipant(removeParticipant: User)
}

class Message {
    + Sender: User
    + Text: string
    + Media: Media
    + Type: MessageType
}

class Media {
    + Filename: string
    + MimeType: string
    + Size: long
    + Width: int?
    + Height: int?
}

class Relation {
    + Type: RelationType
    + Self: User
    + Friend: User
}

enum UserRole {
    USER
    ADMIN
}

enum MessageType {
    TEXT
    MEDIA
}

enum RelationType {
    INCOMING
    OUTGOING
    ESTABLISHED
}

User "1" -- "0..*" MessengerEntry
User "1" -- "0..*" Message
User "1" -- "0..*" Relation
MessengerEntry "1" -- "0..*" Message
Message "0..1" -- "0..1" Media
User "1" -- "1" Profile
User "1" -- "1" Account

hide empty members

@enduml

