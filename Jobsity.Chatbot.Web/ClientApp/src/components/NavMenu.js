import React, { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';
import { useHistory } from 'react-router';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';

function NavMenu() {
    const [isOpen, setIsOpen] = useState(false);
    const login = useSelector((store) => store.login);
    const history = useHistory();

    useEffect(() => {
        if (!login.isLoged) {
            history.push('/login');
        }
    }, [login]);


    const handlerToggle = () => {
        setIsOpen(!isOpen);
    }

    const renderNavItems = () => {
        if (login.isLoged) {
            return (
                <>
                    <NavItem>
                        <NavLink tag={Link} className="text-dark" to="/">Hi {login.user.name}!</NavLink>
                    </NavItem>
                    <NavItem>
                        <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
                    </NavItem>
                    <NavItem>
                        <NavLink tag={Link} className="text-dark" to="/rooms">Rooms</NavLink>
                    </NavItem>
                    <NavItem>
                        <NavLink tag={Link} className="text-dark" to="/logout">Logout</NavLink>
                    </NavItem>
                </>
            )
        } else {
            return (
                <>
                    <NavItem>
                        <NavLink tag={Link} className="text-dark active" to="/login">Login</NavLink>
                    </NavItem>
                    <NavItem>
                        <NavLink tag={Link} className="text-dark" to="/register">Register</NavLink>
                    </NavItem>
                </>
            );
        }
    }

    return (
        <header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3" light>
                <Container>
                    <NavbarBrand tag={Link} to="/">Jobsity.Chatbot.Web</NavbarBrand>
                    <NavbarToggler onClick={handlerToggle} className="mr-2" />
                    <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={isOpen} navbar>
                        <ul className="navbar-nav flex-grow">
                            {renderNavItems()}
                        </ul>
                    </Collapse>
                </Container>
            </Navbar>
        </header>
    );
}

export default NavMenu;