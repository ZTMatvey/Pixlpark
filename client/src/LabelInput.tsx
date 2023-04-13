import './input.scss';

type LabelInputProps = {
  title: string;
  handleChange: React.ChangeEventHandler<HTMLInputElement> | undefined;
}

function LabelInput({title, handleChange} : LabelInputProps) {
  return (
    <div className="form__group field">
        <input onChange={handleChange} type="input" className="form__field" placeholder="Name" name="name" id='name' required />
        <label htmlFor="name" className="form__label">{title}</label>
    </div>
  );
}

export default LabelInput;
