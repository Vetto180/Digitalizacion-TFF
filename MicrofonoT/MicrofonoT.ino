int entrada = A0, valor = 0, contador = 1;
void setup()
{
  Serial.begin(250000);
}

void loop()
{
  char permiso = Serial.read();
  if((contador > 1)&&(contador < 3001))
  {
    permiso = 'S';
  }
  else if (contador == 3001)
  {
    contador = 1;
  }
  if((contador <= 3000)&&(permiso == 'S')){
    valor = analogRead(entrada)-425;
    Serial.println(valor);
    contador = contador + 1; 
  }
}
