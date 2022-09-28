import React from 'react';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import * as Yup from 'yup';
import styled from "styled-components";
import { fetchWrapper } from '../Plugins/fetchWrapper';

const Input = styled.input`
  font-size: 18px;
  padding: 10px;
  margin: 10px;
  background: papayawhip;
  border: none;
  border-radius: 3px;
  ::placeholder {
    color: palevioletred;
  }
`;

const HashSchema = Yup.object().shape({
    InitialValues: Yup.string().required('Required')
});


interface Hash {
    InputValue: string,
    HashType: string,
    AttackMethod: string
}

class HashForm extends React.Component {
    render() {
        const initialValues: Hash = {
            InputValue: '',
            HashType: '',
            AttackMethod: ''
        };
        return (
            <div>
                <h1>Any place in your app!</h1>
                <Formik
                    initialValues={initialValues}
                    validate={values => {
                        const errors = {};
                        return errors;
                    }}
                    onSubmit={(values, actions) => {
                        fetch('api/Home/Decode', {
                            method: 'POST',
                            headers: { 'Content-type': 'application/json' },
                            body: JSON.stringify( values )
                        }).then(r => r.json()).then(res => {
                            if (res) {
                                console.log(res);
                            }
                        });
                        actions.setSubmitting(false);
                    }}
                >

                    {({ isSubmitting }) => (
                        <Form>
                            <div>
                                <Field
                                    name="InputValue"
                                    render={({ field, form: { touched, errors } }) => (
                                        <div>
                                            <Input {...field} type="text" placeholder="fedd1d1122aa65028c81e16ceb85d9c73790a2fa" />
                                            {touched[field.name] && errors[field.name] && <div className="error">{errors[field.name]}</div>}
                                        </div>
                                    )}
                                />
                            </div>
                            <div>
                                <Field name="HashType" as="select">
                                    <option value="100">SHA1</option>
                                    <option value="0">MD5</option>
                                    <option value="900">MD4</option>
                                    <option value="1700">SHA2-512</option>
                                </Field>
                            </div>
                            <div>
                                <Field name="AttackMethod" as="select">
                                    <option value="0">Straight</option>
                                    <option value="1">Combination</option>
                                    <option value="3">Brute-Force</option>
                                </Field>
                            </div>
                            <div>
                                <button type="submit" disabled={isSubmitting}>
                                    Submit
                                </button>
                            </div>
                        </Form>
                    )}
                </Formik>
            </div>
        );
    }
}

export default HashForm;