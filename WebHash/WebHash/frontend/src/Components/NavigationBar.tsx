import React from 'react';
import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';


const styleNavBar = {
    margin: {
        marginLeft: 0
    }
}

class NavigationBar extends React.Component {
    render() {
        return (
            <>
                <Navbar bg="dark" variant="dark" >
                    <Container style={styleNavBar.margin}>
                        <Navbar.Brand href="#home">
                            <img
                                alt=""
                                src="/logo.svg"
                                width="30"
                                height="30"
                                className="d-inline-block align-top"
                            />{' '}
                            WebHash
                        </Navbar.Brand>
                        <Nav className="me-auto">
                            <Nav.Link href="#home">Crack</Nav.Link>
                            <Nav.Link href="#features">Files</Nav.Link>
                            <Nav.Link href="#pricing">Information</Nav.Link>
                        </Nav>
                    </Container>
                </Navbar>
            </>
        );
    }

}

export default NavigationBar;