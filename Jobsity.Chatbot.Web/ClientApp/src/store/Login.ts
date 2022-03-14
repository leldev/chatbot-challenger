import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface LoginState {
    user: object;
    isLoged: boolean;
    error: string;
    errorRegister: string;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.
// Use @typeName and isActionType for type detection that works even after serialization/deserialization.

export interface LoginAction { type: 'LOGIN', data: {} }
export interface RegisterAction { type: 'REGISTER', data: {} }
export interface LogoutAction { type: 'LOGOUT' }

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
export type KnownAction = LoginAction | LogoutAction | RegisterAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    login: (name: string, password: string): AppThunkAction<KnownAction> => (dispatch) => {
        fetch(`http://localhost:16080/api/login`, {
            method: 'post',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                name: name,
                password: password
            }),
        })
            .then((response) => response.json())
            .then((data) => {
                dispatch({ type: 'LOGIN', data: data });
            }).catch((error) => {
                console.error(error)
            });
    },
    register: (name: string, password: string): AppThunkAction<KnownAction> => (dispatch) => {
        fetch(`http://localhost:16080/api/chatusers`, {
            method: 'post',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                name: name,
                password: password
            }),
        })
            .then((response) => response.json())
            .then((data) => {
                console.log(data);
                dispatch({ type: 'REGISTER', data: data });
            }).catch((error) => {
                console.error(error)
            });
    },
    logout: () => ({ type: 'LOGOUT' } as LogoutAction)
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

export const reducer: Reducer<LoginState> = (state: LoginState | undefined, action: any): LoginState => {
    if (state === undefined) {
        return { user: {}, isLoged: false, error: '', errorRegister: '' };
    }

    switch (action.type) {
        case 'LOGIN':
            if (action.data.status) {
                // error
                return { ...state, error: action.data };
            } else {
                return { user: action.data, isLoged: true, error: '', errorRegister: '' };
            }
        case 'REGISTER':
            if (action.data.status) {
                // error
                return { ...state, errorRegister: action.data };
            } else {
                return { user: action.data, isLoged: true, error: '', errorRegister: '' };
            }
        case 'LOGOUT':
            return { user: {}, isLoged: false, error: '', errorRegister: '' };
        default:
            return state;
    }
};
