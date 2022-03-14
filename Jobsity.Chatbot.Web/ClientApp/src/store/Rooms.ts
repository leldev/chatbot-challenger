import { Reducer } from 'redux';
import { AppThunkAction } from './';
import { orderBy } from 'lodash';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface RoomsState {
    rooms: any[];
    isLoading: boolean,
    room: any,
    chatBot: any[],
    error: any;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.
// Use @typeName and isActionType for type detection that works even after serialization/deserialization.

export interface GetAllAction { type: 'GETALLROOMS', data: [] }
export interface GetByIdAction { type: 'GETBYIDROOM', data: {} }
export interface CreateRoomAction { type: 'CREATEROOM', data: {} }
export interface CreateRoomChatAction { type: 'CREATEROOMCHAT', data: {}, payload: {} }
export interface CleanRoomAction { type: 'CLEANROOMS' }

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
export type KnownAction = GetAllAction | GetByIdAction | CreateRoomAction | CreateRoomChatAction | CleanRoomAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    getAllRooms: (): AppThunkAction<KnownAction> => (dispatch) => {
        fetch(`http://localhost:16080/api/rooms`, {
            method: 'get',
            headers: { 'Content-Type': 'application/json' }
        })
            .then((response) => response.json())
            .then((data) => {
                console.log(data);
                dispatch({ type: 'GETALLROOMS', data: data });
            }).catch((error) => {
                console.log(error)
            });
    },
    getByIdRoom: (roomId: string): AppThunkAction<KnownAction> => (dispatch) => {
        fetch(`http://localhost:16080/api/rooms/${roomId}`, {
            method: 'get',
            headers: { 'Content-Type': 'application/json' }
        })
            .then((response) => response.json())
            .then((data) => {
                console.log(data);
                dispatch({ type: 'GETBYIDROOM', data: data });
            }).catch((error) => {
                console.log(error)
            });
    },
    createRoom: (name: string): AppThunkAction<KnownAction> => (dispatch) => {
        fetch(`http://localhost:16080/api/rooms`, {
            method: 'post',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                name: name
            }),
        })
            .then((response) => response.json())
            .then((data) => {
                console.log(data);
                dispatch({ type: 'CREATEROOM', data: data });
            }).catch((error) => {
                console.log(error)
            });
    },
    createRoomChat: (roomId: string, message: string, userId: string, userName: string): AppThunkAction<KnownAction> => (dispatch) => {
        let body = {
            message: message,
            userId: userId,
            userName: userName
        };
        fetch(`http://localhost:16080/api/rooms/${roomId}/chats`, {
            method: 'post',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(body),
        })
            .then((response) => response.json())
            .then((data) => {
                console.log(data);
                dispatch({ type: 'CREATEROOMCHAT', data: data, payload: body });
            }).catch((error) => {
                console.log(error)
            });
    },
    cleamRooms: (): AppThunkAction<KnownAction> => (dispatch) => {
        dispatch({ type: 'CLEANROOMS' });
    },
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

export const reducer: Reducer<RoomsState> = (state: RoomsState | undefined, action: any): RoomsState => {
    if (state === undefined) {
        return { rooms: [], isLoading: true, room: null, error: '', chatBot: [] };
    }

    switch (action.type) {
        case 'GETALLROOMS':
            return { ...state, rooms: action.data, isLoading: false };
        case 'CLEANROOMS':
            return { rooms: [], isLoading: true, room: null, error: '', chatBot: [] };
        case 'GETBYIDROOM':
            if (action.data.status) {
                // error
                return { ...state, error: action.data };
            } else {
                let _rooms = [...state.rooms];
                let room = _rooms.find((x) => x.id === action.data.id);
                room.chats = action.data.chats;
                room.isLoaded = true;

                return { ...state, rooms: _rooms, room: null, error: '', chatBot: [] };
            }
        case 'CREATEROOM':
            if (action.data.status) {
                // error
                return { ...state, error: action.data };
            } else {
                let _rooms = [...state.rooms, action.data];
                return { ...state, rooms: _rooms, room: action.data, error: '' };
            }
        case 'CREATEROOMCHAT':
            if (action.data.status) {
                // error
                return { ...state, error: action.data };
            } else {
                let _rooms = [...state.rooms];
                let room = _rooms.find((x) => x.id === action.data.id);
                room.chats = action.data.chats;
                room.isLoaded = true;

                // Chat bots
                let _chatBot = [...state.chatBot];
                console.log(_chatBot)
                console.log(room.chats)
                let lastChat = room.chats[room.chats.length - 1];
                console.log(lastChat)
                if (lastChat.userId === 'chatbot-stock') {
                    _chatBot.push({ ...action.payload, createdDate: lastChat.createdDate });
                    _chatBot.push(lastChat);
                    room.chats.pop();
                }

                room.chats = room.chats.concat(_chatBot);
                room.chats = orderBy(room.chats, ['createdDate'], ['asc']);

                return { ...state, rooms: _rooms, room: null, error: '', chatBot: _chatBot };
            }
        default:
            return state;
    }
};
