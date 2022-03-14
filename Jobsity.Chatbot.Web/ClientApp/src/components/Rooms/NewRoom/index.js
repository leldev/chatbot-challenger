import React, { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import styled from 'styled-components';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Input, Spinner, Alert } from 'reactstrap';
import { actionCreators } from '../../../store/Rooms';

function NewRoom() {
  const [showModal, setShowModal] = useState(false);
  const [nameValue, setNameValue] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState('');
  const ROOMNAMEMAXLENGTH = 40;

  const rooms = useSelector((x) => x.rooms);
  const dispatch = useDispatch();

  useEffect(() => {
    if (rooms.error) {
      setErrorMessage('New room error, try again.');
      setIsLoading(false);
    }
    if (rooms.room) {
      setShowModal(false);
      resetState();
    }
  }, [rooms])

  const resetState = () => {
    setShowModal(false);
    setNameValue('');
    setIsLoading(false);
  }

  const handlerOnNewRoomClick = () => {
    setErrorMessage('');
    setShowModal(true);
  }

  const handlerOnCloseModal = () => {
    resetState();
  }

  const handlerOnSaveModal = () => {
    dispatch(actionCreators.createRoom(nameValue));
    setIsLoading(true);
  }

  const handlerOnNameChange = (e) => {
    setNameValue(e.target.value)
    setErrorMessage('');
  }

  const renederNewRoomButton = () => {
    const closeButtonProps = {
      id: 'btnNewRoomId',
      onClick: handlerOnNewRoomClick
    };

    return (
      <Button {...closeButtonProps}>
        + New room
      </Button>
    );
  }

  const renderModalFooter = () => {
    const saveButtonProps = {
      variant: 'secondary',
      id: 'btnNewRoomSaveId',
      disabled: isLoading,
      onClick: handlerOnSaveModal
    };

    const closeButtonProps = {
      variant: 'secondary',
      id: 'btnNewRoomCloseId',
      disabled: isLoading,
      onClick: handlerOnCloseModal
    };

    const loading = <Spinner animation="grow" size="sm" />;

    return (
      <StyledModalFooter>
        <Button {...saveButtonProps}>
          {isLoading && loading} Save
        </Button>
        <Button {...closeButtonProps}>
          Close
        </Button>
      </StyledModalFooter>
    );
  }

  const renderModalHeader = () => {
    return (<span>Create new room</span>)
  }

  const renderModalBody = () => {
    const nameProps = {
      type: 'text',
      id: 'txtRoomNameId',
      placeholder: 'Name',
      required: true,
      value: nameValue,
      maxLength: ROOMNAMEMAXLENGTH,
      onChange: handlerOnNameChange
    };

    return (
      <Input {...nameProps} />
    )
  }

  const renderErrorMessage = () => {
    const errorMessageProps = {
      className: 'alert-danger',
    }

    return (<Alert {...errorMessageProps}>{errorMessage}</Alert>)
  }

  return (
    <StyledNewRoom>
      {renederNewRoomButton()}
      <Modal isOpen={showModal} onHide={handlerOnCloseModal}>
        <ModalHeader closeButton>
          {renderModalHeader()}
        </ModalHeader>
        <ModalBody>
          <StyledModalBody>
            {renderModalBody()}
            {errorMessage && renderErrorMessage()}
          </StyledModalBody>
        </ModalBody>
        <ModalFooter>
          {renderModalFooter()}
        </ModalFooter>
      </Modal>
    </StyledNewRoom>
  );
}

const StyledNewRoom = styled.div`
`

const StyledModalBody = styled.div`
  .alert-danger {
    margin-top: 1rem;
  }
`

const StyledModalFooter = styled.div`
  #btnNewRoomSaveId {
    margin-right: 1rem;
  }
`

export default NewRoom;