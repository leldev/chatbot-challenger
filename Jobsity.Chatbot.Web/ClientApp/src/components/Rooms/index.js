import React, { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { useHistory, useParams } from 'react-router';
import styled from 'styled-components';
import { isEmpty } from 'lodash';
import { Card, ListGroup, ListGroupItem, Button, Spinner } from 'reactstrap';
import ChatRoom from './ChatRoom';
import NewRoom from './NewRoom';
import { actionCreators } from '../../store/Rooms';

function Rooms() {
  const [selectedRoom, setSelectedRoom] = useState({});

  const rooms = useSelector((x) => x.rooms);
  const login = useSelector((x) => x.login);
  const dispatch = useDispatch();
  const history = useHistory();
  const { roomId } = useParams();

  useEffect(() => {
    if (login.isLoged) {
      dispatch(actionCreators.getAllRooms());
    }

    return () => {
      dispatch(actionCreators.cleamRooms());
    }
  }, [])

  const getRoomItems = () => {
    return rooms.rooms.map((room) => {
      const joinButtonProps = {
        id: `btnJoinRoomId-${room.id}`,
        key: room.id,
        size: 'm',
        className: 'link btn-primary',
        onClick: () => handlerOnRoomClick(room)
      };

      return (
        <ListGroupItem key={room.id}>
          <div className='room'>
            <Button {...joinButtonProps} >{'Join >'}</Button>
            <span>{room.name}</span>
          </div>
        </ListGroupItem>
      );
    });
  }

  const getNoRoomItems = () => {
    return (
      <div className='rooms-no-rooms'>
        No rooms
      </div>
    );
  }

  const handlerOnRoomClick = (room) => {
    setSelectedRoom(room);
    history.push(`/rooms/${room.id}`);
  }

  const renderNewRoom = () => {
    return (
      <div className='rooms-new'>
        <NewRoom />
      </div>
    );
  }

  const renderIsLoading = () => {
    return (
      <div className='rooms-loading'>
        <Spinner animation="grow" size="xl" />
      </div>
    );
  }

  const renderRooms = () => {
    let result;

    if (isEmpty(rooms.rooms)) {
      result = getNoRoomItems()
    } else {
      result = getRoomItems()
    }

    return (
      <div className='rooms'>
        <Card>
          <ListGroup flush>
            {result}
          </ListGroup>
        </Card>
      </div>
    );
  }

  const renderRoomChat = () => {
    return roomId && selectedRoom &&
      <div className='rooms-chat'>
        <ChatRoom room={selectedRoom} />
      </div>
  }

  return (
    <StyledRooms>
      {renderNewRoom()}
      {rooms.isLoading ? renderIsLoading() : renderRooms()}
      {renderRoomChat()}
    </StyledRooms>
  );
}

const StyledRooms = styled.div`
  .rooms {
    .list-group {
      max-height: 20rem;
      overflow: auto;
      .list-group-item {
        padding: 0.5rem;
      }
    }
    .room {
      button {
        padding: 0 0.5rem;
        margin-right: 1rem;
      }
    }
    .rooms-no-rooms {
      padding: 0.5rem;
    }
  }
  .rooms-new {
      margin-bottom: 1rem;
  }
  .rooms-loading {
    display: flex;
    justify-content: center;
    .spinner-border-xl {
      width: 6rem;
      height: 6rem;
    }
  }
  .rooms-chat {
    margin-top: 1rem;
  }
`

export default Rooms;