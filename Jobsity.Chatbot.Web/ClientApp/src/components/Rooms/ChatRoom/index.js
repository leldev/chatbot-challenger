import React, { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import PropTypes from 'prop-types';
import styled from 'styled-components';
import { isEmpty } from 'lodash';
import Moment from 'react-moment';
import { Card, CardHeader, CardBody, Input, Badge, Spinner } from 'reactstrap';
import { actionCreators } from '../../../store/Rooms';

function ChatRoom({ room }) {
  const [messageValue, setMessageValue] = useState('');
  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  const MESSAGEMAXLENGTH = 100;

  const rooms = useSelector((x) => x.rooms);
  const login = useSelector((x) => x.login);
  const dispatch = useDispatch();

  useEffect(() => {
    let _room = rooms.rooms.find((x) => x.id === room.id);
    if (_room && _room.isLoaded) {
      resetState();
    } else {
      dispatch(actionCreators.getByIdRoom(room.id));
      setIsLoading(true);
    }
  }, [room.id, rooms])

  const resetState = () => {
    setIsLoading(false);
    setIsSaving(false);
    setMessageValue('');
  }

  const saveMessage = () => {
    dispatch(actionCreators.createRoomChat(room.id, messageValue, login.user.id, login.user.name));
    setIsSaving(true);
  }

  const getNoChatItems = () => {
    return (
      <div className='chatroom-no-chat'>
        No chats
      </div>
    );
  }

  const getChatItems = () => {
    return room.chats.map((chat) => {
      return (
        <div className={`chatroom ${getChatRoomClassName(chat)}`}>
          <div className='chatroom-user'>{chat.userName}</div>
          <div className='chatroom-text'>{chat.message}</div>
          <div className='chatroom-date'><Moment fromNow>{chat.createdDate}</Moment></div>
        </div >
      );
    });
  }

  const getChatRoomClassName = (chat) => {
    switch (chat.userId) {
      case 'chatbot-stock':
        return 'chatroom-bot'
      case login.user.id:
        return 'chatroom-my'
      default:
        return 'chatroom-other'
    }
  }

  const handlerOnMessageChange = (e) => {
    setMessageValue(e.target.value)
  }

  const handlerOnMessageKeyDown = (e) => {
    if (e.keyCode === 13) {
      saveMessage();
    }
  }

  const handlerOnBadgeClick = () => {
    if (!isSaving) {
      saveMessage();
    }
  }

  const renderChatRoom = () => {
    let result;

    if (isEmpty(room.chats)) {
      result = getNoChatItems();
    } else {
      result = getChatItems();
    }

    return (
      <Card>
        <CardHeader>Room: {room.name}</CardHeader>
        <CardBody>
          {result}
        </CardBody>
      </Card>
    );
  }

  const renderIsLoading = () => {
    return (
      <div className='chatroom-loading'>
        <Spinner animation="grow" size="xl" />
      </div>
    );
  }

  const renderAddChat = () => {
    const messageProps = {
      type: 'text',
      id: 'txtChatMessageId',
      placeholder: 'Message',
      required: true,
      value: messageValue,
      disabled: isSaving,
      maxLength: MESSAGEMAXLENGTH,
      onChange: handlerOnMessageChange,
      onKeyDown: handlerOnMessageKeyDown
    };

    const badgeProps = {
      bg: 'info',
      pill: true,
      onClick: handlerOnBadgeClick
    };

    return (
      <div className='chatroom-new-message'>
        <Input {...messageProps} />
        <Badge {...badgeProps}>+</Badge>
      </div>
    )
  }

  return (
    <StyledChatRoom>
      {isLoading ? renderIsLoading() : <>
        {renderChatRoom()}
        {renderAddChat()}
      </>}
    </StyledChatRoom>
  );
}

const StyledChatRoom = styled.div`
  .card-body {
    display: flex;
    flex-direction: column;
    max-height: 20rem;
    overflow: auto;
    .chatroom {
      border: 1px solid rgba(0, 0, 0, 0.125);
      border-radius: calc(0.25rem - 1px);
      margin-bottom: 1rem;
      max-width: 55%;
      padding: 0.5rem;
      .chatroom-user {
        font-weight: bold;
      }
      .chatroom-date {
        color: lightgray;
      text-align: right;
      }
    }
    .chatroom-my {
      margin-left: 50%;
      background-color: rgba(0, 0, 0, 0.03);
      .chatroom-user {
        display: none
      }
    }
    .chatroom-bot {
      max-width: 100%;
      background-color: #6c757d;
      color: white;
    }
  }
  .chatroom-new-message {
    display: flex;
    .badge {
      display: inline-flex;
      align-items: center;
      font-size: 1rem;
    }
    .badge:hover {
      background: blue;
      cursor: pointer;
    }
  }
  .chatroom-loading {
    display: flex;
    justify-content: center;
    padding-top: 2rem;
    .spinner-border-xl {
      color: blue;
      width: 4rem;
      height: 4rem;
    }
  }
`

ChatRoom.propTypes = {
  room: PropTypes.shape({
    id: PropTypes.string.isRequired,
    name: PropTypes.string.isRequired
  }).isRequired
}

ChatRoom.defaultProps = {
  roomId: {}
}

export default ChatRoom;