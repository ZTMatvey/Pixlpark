import './button.scss';

type CustomButtonProps = {
  content: string;
  onClick: React.MouseEventHandler<HTMLButtonElement> | undefined;
}

function CustomButton({content, onClick} : CustomButtonProps) {
  return (
    <button onClick={onClick} className="button-23" role="button">{content}</button>
  );
}

export default CustomButton;
