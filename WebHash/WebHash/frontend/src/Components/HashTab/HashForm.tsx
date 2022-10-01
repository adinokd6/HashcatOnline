import React from 'react';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import * as Yup from 'yup';
import styled from "styled-components";
import { fetchWrapper } from '../../Plugins/fetchWrapper';
import CustomSelect from '../Tools/Tools';

const Input = styled.input`
  font-size: 18px;
  padding: 10px;
  margin: 10px;
  background: rgb(33, 37, 41);
  border: none;
  color: yellow;
  border-radius: 3px;
  ::placeholder {
    color: yellow;
  }
`;

const HashSchema = Yup.object().shape({
    InitialValues: Yup.string().required('Required')
});


const hashTypeOptions = [
    {
        label: "SHA1",
        value: 100
    }
]

interface Hash {
    InputValue: string,
    HashType: string,
    AttackMethod: string,
    FirstDictionary: string,
    SecondDictionary: string
}

class HashForm extends React.Component {
    render() {
        const initialValues: Hash = {
            InputValue: '',
            HashType: '',
            AttackMethod: '',
            FirstDictionary: '',
            SecondDictionary: ''
        };
        return (
                <Formik
                    initialValues={initialValues}
                    validate={values => {
                        const errors = {};
                        return errors;
                    }}
                    onSubmit={(values, actions) => {
                        fetchWrapper.post('api/Home/Decode', values)
                            .then(data => console.log('Success!', data))
                            .catch(error => console.error('There was an error!', error));
                        actions.setSubmitting(false);
                    }}
                >

                    {({ isSubmitting }) => (
                        <Form>
                            <div>
                                <Field
                                    name="InputValue"
                                    render={({ field, form: { touched, errors } }) => (
                                        <div >
                                            <Input {...field} style={{ width: "250px" }} type="text" placeholder="fedd1d1122aa65028c81e16ceb85d9c73790a2fa" />
                                            {touched[field.name] && errors[field.name] && <div className="error">{errors[field.name]}</div>}
                                        </div>
                                    )}
                                />
                            </div>
                            <div style={{width: "250px", display: "inline-table"}}>
                                <Field
                                    className="custom-select"
                                    name="HashType"
                                    options={hashTypeOptions}
                                    component={CustomSelect}
                                    placeholder="Select hash type"
                                    isMulti={false}
                                />
                            </div>
                            <div>
                                <Field name="AttackMethod" type="number" as="select">
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
        );
    }
}

export default HashForm;